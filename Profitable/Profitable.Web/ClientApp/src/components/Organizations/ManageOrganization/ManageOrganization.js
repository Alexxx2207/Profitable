import { debounce } from "@mui/material";
import { useContext, useEffect, useReducer } from "react";
import { useNavigate } from "react-router-dom";
import { organizationRolesToManage, searchedModels, SEARCH_ENTITY_IN_PAGE_COUNT } from "../../../common/config";
import { AuthContext } from "../../../contexts/AuthContext";
import { MessageBoxContext } from "../../../contexts/MessageBoxContext";
import { getOrganization, updateOrganization } from "../../../services/organization/organizationsService";
import { searchByTerm } from "../../../services/search/searchService";
import { MemberSearchResult } from "./MemberSearchResult/MemberSearchResult";

import { getUserDataByJWT } from "../../../services/users/usersService";
import { useParams } from "react-router-dom";

import styles from "./ManageOrganization.module.css";

const reducer = (state, action) => {
    switch (action.type) {
        case "changeUserManager":
            return {
                ...state,
                userManager: action.payload
            }
        case "changeOrganizationName":
            return {
                ...state,
                organizationName: action.payload
            }
        case "setShowSaveOnName":
            return {
                ...state,
                showSaveOnName: action.payload
            }
        case "setShowMoreButton":
            return {
                ...state,
                showShowMore: action.payload
            }
        case "changeSearchResults":
            return {
                ...state,
                peopleFound: action.payload
            }
        default:
            return {
                ...state
            }
    }
};

export const ManageOrganization = () => {

    const { organizationId } = useParams();

    const navigate = useNavigate();

    const { JWT } = useContext(AuthContext);

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const [pageState, setState] = useReducer(reducer, {
        userManager: "",
        organizationName: "",
        showSaveOnName: false,
        peopleFound: [],
        searchTerm: "",
        showShowMore: false,
    });

    useEffect(() => {
        getUserDataByJWT(JWT)
        .then(user => {
            if(!user) {
                navigate("/login");
            }
            
            if(user.organizationId !== organizationId) {
                navigate(`/organization/${user.organizationId}/manage`);
            }

            if(user.organizationRole.localeCompare(organizationRolesToManage.Admin) !== 0 &&
                user.organizationRole.localeCompare(organizationRolesToManage.Owner) !== 0) {
                    navigate("/organization");
            }
            getOrganization(organizationId)
                .then(organization => {
                    setState({
                        type: "changeOrganizationName",
                        payload: organization.name
                    });
                    setState({
                        type: "changeUserManager",
                        payload: {...user}
                    });
                })
        });
    }, [JWT, organizationId, organizationRolesToManage]);

    const changeNameOnChangeHandler = (e) => {
        setState({
            type: "changeOrganizationName",
            payload: e.target.value
        });
        setState({
            type: "setShowSaveOnName",
            payload: e.target.value !== pageState.organizationName
        });
    };
   
    const saveNameOnClickHandler = (e) => {
        if( pageState.organizationName.length > 0)
        {
            setState({
                type: "setShowSaveOnName",
                payload: false
            });
            updateOrganization(JWT, pageState.organizationName, organizationId)
                .then(result => {
                    setMessageBoxSettings(
                        `Organization name was updated successfully`,
                        true
                    );
                });
        } else {
            setMessageBoxSettings(
                `Cannot name organization with no characters`,
                false
            );
        }
    };

    const searchTermOnChange = (e) => {
        setState({
            type: "changeSearchTerm",
            payload: e.target.value,
        });
        searchByTerm(e.target.value, searchedModels.Users, 0, SEARCH_ENTITY_IN_PAGE_COUNT).then(
            (result) => {
                setState({
                    type: "changeSearchResults",
                    payload: [...result.list],
                });

                if (result.list.length <= 0) {
                    setState({
                        type: "setShowMoreButton",
                        payload: false,
                    });
                }
            }
        );
    };

    const optimisedOnChange = debounce(searchTermOnChange, 700);

    return (
        <div className={styles.pageContainer}>
            <h1 className={styles.pageHeader}>Organization Management</h1>
            <div className={styles.pageBody}>
                <div className={styles.nameManagement}>
                    <h5>Name: </h5>
                    <input 
                        className={styles.nameInputField}
                        type="text" 
                        value={pageState.organizationName}
                        onChange={changeNameOnChangeHandler} />

                    {pageState.showSaveOnName ?
                        <button 
                            className={styles.saveButton}
                            onClick={saveNameOnClickHandler}>
                            Save
                        </button>
                    :
                        <h6>* Change the name by writhing the new one in the textbox</h6>    
                    }
                    
                </div>
                <div className={styles.manageMembers}>
                    <h3>Search Members To Add/Remove</h3>
                    <h6>Search From All Users On Profitable</h6>
                    <div className={styles.searchInputContainer}>
                        <input
                            placeholder={"Search here by name or email..."}
                            type="text"
                            onChange={optimisedOnChange}
                            className={styles.searchInput}
                        />
                    </div>
                    <section className={styles.resultSection}>
                        {pageState.peopleFound.length > 0 ? (
                            <>
                                <h2 className={styles.searchedResultsHeader}>Searched Results</h2>
                                <div className={styles.searchedResultsContainer}>
                                    <div className={styles.userResultsContainer}>
                                        {pageState.peopleFound.map((person, index) => (
                                            <MemberSearchResult 
                                                key={person.guid}
                                                JWT={JWT}
                                                user={person} 
                                                organizationId={organizationId}
                                                userManager={pageState.userManager}/>
                                        ))}
                                    </div>
                                </div>
                            </>
                        ) : (
                            <h2 className={styles.noMatchingHeader}>User Not Found</h2>
                        )}
                    </section>
                </div>
            </div>
        </div>
    );
}