import classNames from 'classnames';
import { useContext } from 'react';
import { NavLink } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';
import styles from './NavBar.module.css';

export const NavBar = () => {

    const { JWT } = useContext(AuthContext);

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
                            <NavLink to="/user-profile" className={classNames(styles.navbarListItems, styles.navLink)}>PROFILE</NavLink>
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