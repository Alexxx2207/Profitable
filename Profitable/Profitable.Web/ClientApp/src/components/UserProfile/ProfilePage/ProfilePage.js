import { useEffect, useState, useContext } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { createAuthorImgURL } from '../../../services/common/imageService';
import { AuthContext } from '../../../contexts/AuthContext';
import { getUserDataByEmail, getUserEmailFromJWT } from '../../../services/users/usersService';
import { EditUser } from "../EditUser/EditUser";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit } from '@fortawesome/free-solid-svg-icons';

import { USER_NOT_FOUND_ERROR_PAGE_PATH } from '../../../common/config';

import styles from './ProfilePage.module.css';

export const ProfilePage = () => {

    const navigate = useNavigate();

    const location = useLocation();

    const { searchedProfileEmail } = useParams();

    const [ profileInfo, setProfileInfo ] = useState({});

    const [ loggedInUserEmail, setLoggedInUserEmail] = useState({});

    const { JWT, removeJWT } = useContext(AuthContext);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(result => setLoggedInUserEmail(email => result))
            .catch(err => {
                removeJWT();
                navigate(location.pathname);
            })
    // eslint-disable-next-line
    }, []);

    useEffect(() => {
        getUserDataByEmail(searchedProfileEmail)
            .then(result => setProfileInfo(result))
            .catch(err => navigate(USER_NOT_FOUND_ERROR_PAGE_PATH))
    }, [JWT, searchedProfileEmail, navigate]);

    const changeProfileInfo = (user) => {
        setProfileInfo(state => ({
            ...user
        }))
    }

    return (
        <div className={styles.profilePageContainer}>

            <div className={styles.imageContainer}>
                <img className={styles.authorImage} src={createAuthorImgURL(profileInfo.profileImage)} alt="" />
            </div>
            <div className={styles.profileInfo}>

                <div className={styles.userNameContainer}>
                    <h1 className={styles.userName}>{profileInfo.firstName} {profileInfo.lastName}</h1>
                </div>
                <div className={styles.emailContainer}>
                    <h3 className={styles.email}>{profileInfo.email}</h3>
                </div>
                <div className={styles.descriptionContainer}>
                    <div className={styles.description}>
                        {profileInfo.description ?
                            profileInfo.description.split('\\n').map((paragraph, index) =>
                                <h4 key={index}>{paragraph}<br /></h4>
                            )
                            :
                            <h4>"No user description provided!"</h4>
                        }
                    </div>
                </div>
            </div>
            {searchedProfileEmail === loggedInUserEmail ?
                <div className={styles.userSection}>
                    <h1 className={styles.userPrivateZoneHeading}>User Private Zone</h1>
                    <div className={styles.editContainer}>
                        <div className={styles.editHeadingContainer}>
                            <h3 className={styles.editHeadingText}>Edit Section</h3>
                            <FontAwesomeIcon icon={faEdit} className={styles.editIcon} />
                        </div>
                        <div className={styles.editForm}>
                            <EditUser changeProfileInfo={changeProfileInfo} searchedProfileEmail={searchedProfileEmail} />
                        </div>
                    </div>
                </div>
                :
                ''
            }
        </div>
    );
}