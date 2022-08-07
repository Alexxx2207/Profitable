import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { createAuthorImgURL } from '../../services/common/imageService';
import { AuthContext } from '../../contexts/AuthContext';
import { getUserData } from '../../services/users/usersService';
import styles from './ProfilePage.module.css';


export const ProfilePage = () => {

    const navigate = useNavigate();

    const [profileInfo, setProfileInfo] = useState({});
    const { JWT, removeJWT } = useContext(AuthContext);

    useEffect(() => {
        getUserData(JWT)
            .then(result => setProfileInfo(result))
            .catch(err => {
                removeJWT();
                navigate('/login');
            })
    }, [JWT, removeJWT, navigate]);

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
                            <h5 key={index}>{paragraph}<br /></h5>
                            )
                        : 
                            <h5>"No user description provided!"</h5>
                        }
                    </div>
                </div>
            </div>
        </div>
    );
}