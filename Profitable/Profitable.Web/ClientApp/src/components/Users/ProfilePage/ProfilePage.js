import { NavLink, Outlet, useLocation, useParams, useNavigate } from "react-router-dom";
import React, { useEffect, useState, useContext } from "react";

import { AuthContext } from "../../../contexts/AuthContext";

import { GoToTop } from "../../Common/GoToTop/GoToTop";

import styles from "./ProfilePage.module.css";
import { 
    getUserEmailFromJWT,
    getAuthenticatedUserOrganization 
} from "../../../services/users/usersService";

const createOrganizationStates = {
    hide: "hide",
    disable: "disable",
    show: "show",
}

export const ProfilePage = () => {

    const { JWT, removeAuth } = useContext(AuthContext);

    const { searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    const location = useLocation();

    const [url, setUrl] = useState("");

    const [createOrganizationState, setCreateOrganizationState] = 
        useState(createOrganizationStates.hide);

    const [loggedInUserEmail, setLoggedInUserEmail] = useState("");

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then((email) => {
                setLoggedInUserEmail((old) => email);
                if(searchedProfileEmail.localeCompare(email) !== 0)
                {
                    setCreateOrganizationState(createOrganizationStates.hide)
                } else {
                    getAuthenticatedUserOrganization(JWT, email)
                    .then(guid => {
                        if(guid !== "")
                        {
                            setCreateOrganizationState(createOrganizationStates.disable);
                        } else {
                            setCreateOrganizationState(createOrganizationStates.show);
                        }
                    })   
                }
            })
            .catch((err) => {
                removeAuth();
                navigate(location.pathname);
            });
        // eslint-disable-next-line
    }, [searchedProfileEmail]);

    useEffect(() => {
        setUrl(location.pathname);
    }, [location]);

    const createOrganizationButtonClickHandler = (e) => {
        navigate(`/${searchedProfileEmail}/organizations/add`);
    };

    return (
        <div className={styles.profilePageContainer}>
            {createOrganizationState.localeCompare(createOrganizationStates.hide) === 0
            ?
                ''
            :
                <div className={styles.organizationContainer}>
                    {createOrganizationState
                                .localeCompare(createOrganizationStates.disable) === 0
                                ?
                                    <h5 className={styles.organizationMessage}>
                                        You are already in an organization
                                    </h5>
                                :
                                    ""
                                }
                    <button 
                        className={styles.createOrganizationButton}
                        onClick={createOrganizationButtonClickHandler}
                        disabled={
                            createOrganizationState
                                .localeCompare(createOrganizationStates.disable) === 0}>
                            Create Organization
                    </button>
                </div>
            }
           
            <nav className={styles.profilePageNavbarContainer}>
                <NavLink
                    to={"personal-info"}
                    className={url.includes("personal-info") ? styles.active : ""}
                >
                    Personal
                </NavLink>
                <NavLink
                    to={"account-statistics"}
                    className={url.includes("account-statistics") ? styles.active : ""}
                >
                    Statistics
                </NavLink>
            </nav>
            <Outlet />
            <GoToTop />
        </div>
    );
};
