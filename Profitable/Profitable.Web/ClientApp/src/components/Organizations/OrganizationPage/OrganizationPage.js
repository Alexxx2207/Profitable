import { useCallback, useContext, useEffect, useReducer, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { HubConnectionBuilder } from '@microsoft/signalr';
import { HUB_URL, MESSAGES_IN_PAGE_COUNT, organizationRolesToManage } from "../../../common/config";
import { AuthContext } from "../../../contexts/AuthContext";
import { addOrganizationMessage, deleteOrganizationMessage, getOrganization, getOrganizationMessages } from "../../../services/organization/organizationsService";
import { getUserDataByJWT, getAuthenticatedUserOrganization } from "../../../services/users/usersService";
import { OrganizationMessage } from "./OrganizationMessage/OrganizationMessage";

import { ShowMoreButton } from "../../Common/ShowMoreButton/ShowMoreButton";

import { MessageBoxContext } from "../../../contexts/MessageBoxContext";

import styles from "./OrganizationPage.module.css";
import { TimeContext } from "../../../contexts/TimeContext";
import { convertFullDateTime } from "../../../utils/Formatters/timeFormatter";

const reducer = (state, action) => {
    switch (action.type) {
        case "loadOrganizationName":
            return {
                ...state,
                name: action.payload
            }
        case "loadOrganizationMessages":
            return {
                ...state,
                messages: action.payload
            }
        case "receiveMessages":
            return {
                ...state,
                messages: [ ...state.messages, action.payload]
            }
        case "loadMoreOrganizationMessages":
            return {
                ...state,
                messages: [ ...action.payload, ...state.messages]
            }
        case "changeUser":
            return {
                ...state,
                user: action.payload
            }
        case "changeUserOrganizationId":
            return {
                ...state,
                organizationId: action.payload
            }
        case "changeShowManagementButton":
            return {
                ...state,
                showManagementButton: action.payload
            }
        case "changeAddMessageText":
            return {
                ...state,
                addMessageText: action.payload
            }
        case "pageIncrease":
            return {
                ...state,
                page: state.page + 1
            }
        case "setShowMoreButton":
            return {
                ...state,
                showShowMore: action.payload,
            };
        default:
            return {
                ...state
            }
    }
};

export const OrganizationPage = () => {

    const { timeOffset } = useContext(TimeContext);

    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);
    
    const [ connection, setConnection ] = useState(null);

    
    const [state, setState] = useReducer(reducer, {
        name: "",
        messages: [],
        organizationId: "",
        user: "",
        showManagementButton: false,
        addMessageText: "",
        showShowMore: true,
        showEditButton: true,
        page: 0,
    });
    
    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
    
                    connection.on('ReceiveMessage', message => {
                        message.sentOn =  convertFullDateTime(
                            new Date(
                                new Date(message.sentOn).getTime() - timeOffset * 60000
                            )
                        );
                        setState({
                            type: "receiveMessages",
                            payload: message
                        });
                        setTimeout(() => {
                            window.scrollTo(0, document.body.scrollHeight);
                        }, 100)
                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    useEffect(() => {
        async function fetchData() {
            var user = await getUserDataByJWT(JWT);
            var organizationId = await getAuthenticatedUserOrganization(JWT, user.email);

            if(organizationId) {
                var organization = await getOrganization(organizationId);
                var messages = await getOrganizationMessages(JWT, organizationId, state.page, MESSAGES_IN_PAGE_COUNT);
                
                var messagesWithOffsetedTime = [
                    ...messages.map((message) => ({
                        ...message,
                        sentOn: convertFullDateTime(
                            new Date(
                                new Date(message.sentOn).getTime() - timeOffset * 60000
                            )
                        ),
                    })),
                ];

                setState({
                    type: "loadOrganizationName",
                    payload: organization.name
                });
    
                setState({
                    type: "changeUserOrganizationId",
                    payload: organizationId
                });
                
                setState({
                    type: "loadOrganizationMessages",
                    payload: [...messagesWithOffsetedTime]
                });
            }
            
            setState({
                type: "changeUser",
                payload: {...user}
            });
        }
        
        fetchData();
    }, [JWT, MESSAGES_IN_PAGE_COUNT]);

    const addMessageOnChangeHandler = (e) => {
        setState({
            type: "changeAddMessageText",
            payload: e.target.value
        })
    };

    const sendMessage = async (message) => {
        if (connection.connectionId) {
            await connection.send('SendMessage', message);
        }
    }

    const addMessageSendOnClickHandler = (e) => {
        if(state.addMessageText)
        {
            addOrganizationMessage(JWT, state.organizationId, state.addMessageText)
            .then(message => {
                sendMessage(message);
                setState({
                    type: "changeAddMessageText",
                    payload: ""
                });
            });
        }
    };

    const loadMessagesOnShowMoreClick = useCallback(async (page, pageCount) => {
        var messages = await getOrganizationMessages(JWT, state.organizationId, page, pageCount);
        if(messages.length > 0) {
            setState({
                type: "loadMoreOrganizationMessages",
                payload: [...messages]
            });
        } else {
            setMessageBoxSettings(
                `There are no more messages`,
                false
            );

            setState({
                type: "setShowMoreButton",
                payload: false
            });
        }
    }, [JWT, state.organizationId, setMessageBoxSettings]);

    const handleEditOrganizationClick = useCallback((e) => {
        e.preventDefault();
        navigate(`/organizations/${state.organizationId}/manage`);
    }, [state.organizationId, navigate]);
    
    const handleDeleteOrganizationClick = useCallback((e) => {
        e.preventDefault();
        deleteOrganizationMessage(JWT, state.organizationId)
            .then(result => {
                setMessageBoxSettings(
                    `Organization was deleted successfully!`,
                    true
                );
                navigate("/");
            })
            .catch(err => {
                console.log(err.message);
                setMessageBoxSettings(
                    `Organization was not deleted successfully!`,
                    false
                );
            });

    }, [JWT, state.organizationId, navigate, setMessageBoxSettings]);
    
    const handleShowMoreClick = useCallback(
        (e) => {
            e.preventDefault();
            loadMessagesOnShowMoreClick(state.page+1, MESSAGES_IN_PAGE_COUNT)
            setState({
                type: "pageIncrease",
            });
        },
        [loadMessagesOnShowMoreClick, state.page]
    );

    return (
        <div className={styles.pageContainer}>
            {state.user.organizationId ? 
                <div className={styles.chatContainer}>
                    <div className={styles.headerContainer}>
                        <h2 className={styles.organizationName}>{state.name}</h2>
                        {
                            (state.user.organizationRole
                                .localeCompare(organizationRolesToManage.Admin) === 0 ||
                            state.user.organizationRole
                                .localeCompare(organizationRolesToManage.Owner) === 0)
                            ?
                                <button 
                                    className={styles.editOrganizationButton}
                                    onClick={handleEditOrganizationClick}>
                                        Edit Organization
                                </button>
                            :
                                <></>
                        }
                        {
                            state.user.organizationRole
                                .localeCompare(organizationRolesToManage.Owner) === 0
                            ?
                                <button 
                                    className={styles.deleteOrganizationButton}
                                    onClick={handleDeleteOrganizationClick}>
                                        Delete Organization
                                </button>
                            :
                                <></>
                        }
                       
                    </div>
                    <div className={styles.messagesContainer}>
                        <ShowMoreButton
                            entity="Messages"
                            showShowMore={state.showShowMore}
                            handler={handleShowMoreClick} />
                        {state.messages.map((m, i) => 
                            <div key={i} 
                                className={m.senderId === state.user.guid ?
                                    styles.rightMessage :
                                    styles.leftMessage}>
                                <OrganizationMessage message={m}/>
                            </div>
                        )}
                    </div>
                    <div className={styles.footerContainer}>
                        <div className={styles.addMessageContainer}>
                            <input 
                                className={styles.addMessageInput}
                                placeholder="Type your message here!"
                                type="text" 
                                value={state.addMessageText}
                                onChange={addMessageOnChangeHandler} />
                                <button 
                                    className={styles.addMessageSendButton}
                                    onClick={addMessageSendOnClickHandler}>
                                    Send
                                </button>
                        </div>
                    </div>
                </div>
            :
                <div className={styles.notInOrganizationContainer}>
                    <h1 className={styles.notInOrganizationMessage}>You are not in an organization</h1>
                    <h4 className={styles.notInOrganizationMessage}>Contact an admin or the owner of your organization to be added</h4>
                </div>
            }
        </div>
    );
}