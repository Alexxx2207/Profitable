import { useEffect, useState, useContext } from 'react';
import { useLocation, useNavigate, useParams } from 'react-router-dom';
import { createAuthorImgURL } from '../../../services/common/imageService';
import { AuthContext } from '../../../contexts/AuthContext';
import { getUserDataByEmail, getUserEmailFromJWT } from '../../../services/users/usersService';
import { EditUser } from "../EditUser/EditUser";
import { EditPassword } from "../EditPassword/EditPassword";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEdit } from '@fortawesome/free-solid-svg-icons';
import { faUserLock } from '@fortawesome/free-solid-svg-icons';

import { editUserImage } from '../../../services/users/usersService';

import { USER_NOT_FOUND_ERROR_PAGE_PATH } from '../../../common/config';

import styles from './ProfilePage.module.css';

export const ProfilePage = () => {

    const navigate = useNavigate();

    const location = useLocation();

    const { searchedProfileEmail } = useParams();

    const [profileInfo, setProfileInfo] = useState({
        previewImage: undefined,
        previewImageFileName: undefined,
    });

    const [loggedInUserEmail, setLoggedInUserEmail] = useState({});

    const { JWT, removeAuth, setMessageBoxSettings } = useContext(AuthContext);

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

        reader.onloadend = () => {
            const base64Image = reader.result.toString();
            const byteArray = base64Image.slice(base64Image.indexOf('base64,') + 'base64,'.length);

            setProfileInfo(state => ({
                ...state,
                previewImage: byteArray,
                previewImageFileName: file.name,
            }));
        }

        reader.readAsDataURL(file);
    }

    const discardClickHandler = () => {
        setProfileInfo(state => ({
            ...state,
            previewImage: undefined,
            previewImageFileName: undefined,
        }));
    }

    const saveClickHandler = () => {

        editUserImage(JWT, profileInfo.previewImageFileName, profileInfo.previewImage)
            .then(result => {
                setProfileInfo(state => ({
                    ...state,
                    profileImage: state.previewImage,
                    previewImage: undefined,
                    previewImageFileName: undefined,
                }));
                setMessageBoxSettings('Profile image was changed successfully!', true, true);
            })
            .catch((err) => {
                setMessageBoxSettings(err.message, false, true);
                navigate('/login');
            });
    }

    return (
        <div className={styles.profilePageContainer}>
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
                    <input type="file" accept="image/*" className={styles.fileInput} onChange={changeImageHandler} />
                </div>
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
                            <h3 className={styles.editHeadingText}>General Information</h3>
                            <FontAwesomeIcon icon={faEdit} className={styles.editIcon} />
                        </div>
                        <div className={styles.editForm}>
                            <EditUser changeProfileInfo={changeProfileInfo} searchedProfileEmail={searchedProfileEmail} />
                        </div>

                    </div>
                    <div className={styles.editContainer}>
                        <div className={styles.editHeadingContainer}>
                            <h3 className={styles.editHeadingText}>Change Password</h3>
                            <FontAwesomeIcon icon={faUserLock} className={styles.editIcon} />
                        </div>
                        <div className={styles.editForm}>
                            <EditPassword />
                        </div>
                    </div>
                </div>
                :
                ''
            }
        </div>
    );
}