import classNames from 'classnames';
import { useContext, useEffect, useState } from 'react';
import { NavLink } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';

import { getUserEmailFromJWT } from '../../services/users/usersService';

import styles from './NavBar.module.css';

export const NavBar = () => {

    const { JWT } = useContext(AuthContext);
    const [email, setEmail] = useState('');

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(result => setEmail(state => result))
            .catch(err => err);
    }, [JWT])

    return (
        <nav className={styles.navbarContainer}>

            <div className={styles.logoWrapper}>
                <NavLink to="/" className={styles.navLink}>
                    <h1 className={styles.navbarLogo}>PROFITABLE</h1>
                </NavLink>
            </div>

            <div className={styles.navButtonsContainers}>
                <div className={styles.pages}>
                    <NavLink to="/markets" className={classNames(styles.navbarListItems, styles.navLink)}>MARKETS</NavLink>

                    <NavLink to="/posts" className={classNames(styles.navbarListItems, styles.navLink)}>POSTS</NavLink>

                </div>
                <div className={styles.userPanel}>
                    {JWT ?
                        <div className={styles.authContainer}>
                            <NavLink to={`/users/${email}`} className={classNames(styles.navbarListItems, styles.navLink)}>PROFILE</NavLink>
                            <NavLink to="/logout" className={classNames(styles.navbarListItems, styles.navLink)}>LOGOUT</NavLink>
                        </div>
                        :
                        <div className={styles.authContainer}>
                            <NavLink to="/login" className={classNames(styles.navbarListItems, styles.navLink)}>LOGIN</NavLink>
                            <NavLink to="/register" className={classNames(styles.navbarListItems, styles.navLink)}>REGISTER</NavLink>
                        </div>
                    }
                </div>
            </div>
        </nav>);
}