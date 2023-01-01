import { useCallback, useContext, useEffect, useReducer } from "react";
import { MESSAGES_IN_PAGE_COUNT, organizationRolesToManage } from "../../../common/config";
import { AuthContext } from "../../../contexts/AuthContext";
import { addOrganizationMessage, deleteOrganizationMessage, getOrganization, getOrganizationMessages } from "../../../services/organization/organizationsService";
import { getUserDataByJWT, getAuthenticatedUserOrganization } from "../../../services/users/usersService";
import { OrganizationMessage } from "./OrganizationMessage/OrganizationMessage";

import { ShowMoreButton } from "../../Common/ShowMoreButton/ShowMoreButton";

import styles from "./OrganizationPage.module.css";
import { useNavigate } from "react-router-dom";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";

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
        case "changeAddCommentText":
            return {
                ...state,
                addCommentText: action.payload
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

    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);
    
    const [state, setState] = useReducer(reducer, {
        name: "",
        messages: [],
        organizationId: "",
        user: "",
        showManagementButton: false,
        addCommentText: "",
        showShowMore: true,
        showEditButton: true,
        page: 0,
    });

    useEffect(() => {
        async function fetchData() {
            var user = await getUserDataByJWT(JWT);
            var organizationId = await getAuthenticatedUserOrganization(JWT, user.email);

            if(organizationId) {
                var organization = await getOrganization(organizationId);
                var messages = await getOrganizationMessages(JWT, organizationId, state.page, MESSAGES_IN_PAGE_COUNT);
                
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
                    payload: [...messages]
                });
            }
            
            setState({
                type: "changeUser",
                payload: {...user}
            });
        }
        
        fetchData();
    }, [JWT, MESSAGES_IN_PAGE_COUNT]);

    useEffect(() => {
        window.scrollTo(0, document.body.scrollHeight);
    }, [state.userId]);

    const addCommentOnChangeHandler = (e) => {
        setState({
            type: "changeAddCommentText",
            payload: e.target.value
        })
    };

    const addCommentSendOnClickHandler = (e) => {
        if(state.addCommentText)
        {
            addOrganizationMessage(JWT, state.organizationId, state.addCommentText)
            .then(result => {
                setState({
                    type: "changeAddCommentText",
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
                        <div className={styles.addCommentContainer}>
                            <input 
                                className={styles.addCommentInput}
                                placeholder="Type your message here!"
                                type="text" 
                                value={state.addCommentText}
                                onChange={addCommentOnChangeHandler} />
                                <button 
                                    className={styles.addCommentSendButton}
                                    onClick={addCommentSendOnClickHandler}>
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