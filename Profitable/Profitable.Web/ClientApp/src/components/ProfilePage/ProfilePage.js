import { useEffect, useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
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
    }, []);

    return (
        <div className={styles.profilePageContainer}>
            <div>
                {profileInfo.firstName}
            </div>
        </div>
    );
}