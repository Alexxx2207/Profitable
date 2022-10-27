import { useEffect, useState, useContext } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { createAuthorImgURL, validateImage } from '../../../services/common/imageService';
import { AuthContext } from '../../../contexts/AuthContext';
import { MessageBoxContext } from '../../../contexts/MessageBoxContext';
import { deleteUserData, deleteUserImage, getUserDataByEmail, getUserEmailFromJWT } from '../../../services/users/usersService';
import { EditUser } from "./EditUser/EditUser";
import { EditPassword } from "./EditPassword/EditPassword";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit, faUserLock, faTriangleExclamation } from '@fortawesome/free-solid-svg-icons';

import { editUserImage } from '../../../services/users/usersService';

import { USER_NOT_FOUND_ERROR_PAGE_PATH } from '../../../common/config';

import styles from './ProfileInfo.module.css';

export const ProfileInfo = () => {

    const navigate = useNavigate();

    const location = useLocation();

    const { searchedProfileEmail } = useParams();

    const [profileInfo, setProfileInfo] = useState({
        previewImage: undefined,
        previewImageFileName: undefined,
    });

    const [loggedInUserEmail, setLoggedInUserEmail] = useState({});

    const { JWT, removeAuth } = useContext(AuthContext);
    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(result => setLoggedInUserEmail(email => result))
            .catch(err => {
                removeAuth();
                navigate(location.pathname);
            })
        // eslint-disable-next-line
    }, []);

    useEffect(() => {
        getUserDataByEmail(searchedProfileEmail)
            .then(result => setProfileInfo({
                ...result,
                previewImage: undefined,
                previewImageFileName: undefined,
            }))
            .catch(err => navigate(USER_NOT_FOUND_ERROR_PAGE_PATH))
    }, [JWT, searchedProfileEmail, navigate]);

    const changeProfileInfo = (user) => {
        setProfileInfo(state => ({
            ...user
        }))
    }

    const changeImageHandler = (e) => {
        const file = e.target.files[0];
        const reader = new FileReader();

        
        if(validateImage(file, setMessageBoxSettings)) {
            const base64 = 'base64,';

            reader.onloadend = () => {
                const base64Image = reader.result.toString();
                const byteArray = base64Image.slice(base64Image.indexOf(base64) + base64.length);
    
                setProfileInfo(state => ({
                    ...state,
                    previewImage: byteArray,
                    previewImageFileName: file.name,
                }));
            }
            reader.readAsDataURL(file);
        }
    };

    const discardClickHandler = () => {
        setProfileInfo(state => ({
            ...state,
            previewImage: undefined,
            previewImageFileName: undefined,
        }));
    };

    const saveClickHandler = () => {

        editUserImage(JWT, profileInfo.previewImageFileName, profileInfo.previewImage)
            .then(result => {
                setProfileInfo(state => ({
                    ...state,
                    profileImage: state.previewImage,
                    previewImage: undefined,
                    previewImageFileName: undefined,
                }));
                setMessageBoxSettings('Profile image was changed successfully!', true);
                window.scrollTo(0, 0);
            })
            .catch((err) => {
                setMessageBoxSettings(err.message, false);
                navigate('/login');
            });
    }

    const deleteImageButtonClickHandler = () => {
        deleteUserImage(JWT)
            .then(result => {
                setMessageBoxSettings(`Your image was deleted successfully!`, true);
                setProfileInfo(state => ({
                    ...state,
                    profileImage: undefined
                }));
                window.scrollTo(0, 0);
            })
            .catch(err => {
                setMessageBoxSettings(`Your image was not deleted, session has expired!`, false);
                setProfileInfo(state => ({
                    ...state,
                    profileImage: undefined
                }));
            });
    }

    const deleteUserButtonClickHandler = () => {
        deleteUserData(JWT)
            .then(result => {
                setMessageBoxSettings(`Your account was deleted successfully!`, true);
                removeAuth();
                navigate('/');
            })
            .catch(err => {
                setMessageBoxSettings(`Your account was not deleted, session has expired!`, false);
                removeAuth();
                navigate('/login');
            });
    }

    return(
        <div className={styles.profileInfoContainer}>
            <div className={styles.imageSection}>
                {profileInfo.previewImage ?
                    <div className={styles.imageSectionButtons}>
                        <button className={styles.discardImageButton} onClick={discardClickHandler}>Discard</button>
                        <button className={styles.saveImageButton} onClick={saveClickHandler}>Save</button>
                    </div>
                    :
                    ''
                }
                <div className={styles.imageContainer}>
                    <img className={styles.authorImage}
                        src={profileInfo.previewImage ?
                            createAuthorImgURL(profileInfo.previewImage)
                            :
                            createAuthorImgURL(profileInfo.profileImage)}
                        alt="" />
                    {searchedProfileEmail === loggedInUserEmail ?
                        <input type="file" accept="image/*" className={styles.fileInput} onChange={changeImageHandler} />
                        :
                        ""
                    }
                </div>
            </div>
            <div className={styles.profileInfo}>
                {searchedProfileEmail === loggedInUserEmail ?
                    <div className={styles.changeImageMessageContainer}>
                        <h6 className={styles.changeImageMessage}>* Click on the circle to change your profile picture</h6>
                    </div>
                    : 
                    ''
                }
                
                <div className={styles.profileOverviewTextContainer}>
                    <div className={styles.userNameContainer}>
                        <h2 className={styles.userName}>{profileInfo.firstName} {profileInfo.lastName}</h2>
                    </div>
                    <div className={styles.emailContainer}>
                        <h4 className={styles.email}>{profileInfo.email}</h4>
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
            </div>
            {searchedProfileEmail === loggedInUserEmail ?
                <div className={styles.userSection}>
                    <h1 className={styles.userPrivateZoneHeading}>User Private Zone</h1>
                    <div className={styles.editContainer}>
                        <div className={styles.editHeadingContainer}>
                            <h2 className={styles.editHeadingText}>General Information</h2>
                            <FontAwesomeIcon icon={faEdit} className={styles.editIcon} />
                        </div>
                        <div className={styles.editForm}>
                            <EditUser changeProfileInfo={changeProfileInfo} searchedProfileEmail={searchedProfileEmail} />
                        </div>
                    </div>
                    <div className={styles.editContainer}>
                        <div className={styles.editHeadingContainer}>
                            <h2 className={styles.editHeadingText}>Change Password</h2>
                            <FontAwesomeIcon icon={faUserLock} className={styles.editIcon} />
                        </div>
                        <div className={styles.editForm}>
                            <EditPassword />
                        </div>
                    </div>
                    <div className={styles.dangerZoneContainer}>
                        <div className={styles.dangerZoneHeadingContainer}>
                            <h2 className={styles.dangerZoneHeadingText}>Danger Zone</h2>
                            <FontAwesomeIcon icon={faTriangleExclamation} className={styles.editIcon} />
                        </div>
                        <div className={styles.dangerZone}>
                            <div className={styles.deleteSection}>
                                <div className={styles.deleteHeadingContainer}>
                                    <h3 className={styles.deleteHeading}>
                                        Delete Account
                                    </h3>
                                    <h5 className={styles.deleteHeading}>Your posts will also be deleted!</h5>
                                </div>
                                <button onClick={deleteUserButtonClickHandler} className={styles.deleteButton}>
                                    Delete Account
                                </button>
                            </div>
                            <div className={styles.deleteSection}>
                                <div className={styles.deleteHeadingContainer}>
                                    <h3 className={styles.deleteHeading}>
                                        Remove Profile Image
                                    </h3>
                                    <h5 className={styles.deleteHeading}>Your profile picture will be the default one!</h5>
                                </div>
                                <button onClick={deleteImageButtonClickHandler} className={styles.deleteButton}>
                                    Remove Profile Image
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                :
                ''
            }
        </div>
    );
}