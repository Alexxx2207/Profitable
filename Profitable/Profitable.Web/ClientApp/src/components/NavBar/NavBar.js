import { useContext, useEffect, useState } from 'react';
import classnames from 'classnames';
import { NavLink, useLocation } from 'react-router-dom';

import classNames from 'classnames';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {
    faRightFromBracket,
    faUser,
    faCalculator,
    faNewspaper,
    faCalendarDays,
    faMagnifyingGlass
} from '@fortawesome/free-solid-svg-icons';

import { AuthContext } from '../../contexts/AuthContext';

import { getUserEmailFromJWT } from '../../services/users/usersService';

import styles from './NavBar.module.css';

export const NavBar = () => {

    const location = useLocation();

    const { JWT } = useContext(AuthContext);
    const [email, setEmail] = useState('');
    const [logoState, setLogoState] = useState(false);

    useEffect(() => {
        window.addEventListener('resize', () => {
            if(window.innerWidth < 900) {
                setLogoState(true);
            } else {
                setLogoState(false);
            }
        })
    }, [])

    useEffect(() => {
        getUserEmailFromJWT(JWT)
            .then(result => setEmail(state => result))
            .catch(err => err);
    }, [JWT]);



    return (
        <nav className={styles.navbarContainer}>

            <div className={styles.logoWrapper}>
                <NavLink to="/" className={styles.navLink}>
                    {
                        logoState ?
                        <img src={process.env.PUBLIC_URL + '/images/trader_logo.png'} alt="img" className={styles.logoImage} />
                        :
                        <h1 className={styles.navbarLogo}>PROFITABLE</h1>
                    }
                </NavLink>
            </div>

            <div className={styles.navButtonsContainers}>
                <div className={styles.pages}>
                    <NavLink to="/markets" className={classNames(styles.navbarListItems, styles.navLink)}>CHARTS</NavLink>

                    <NavLink to="/posts" className={classNames(styles.navbarListItems, styles.navLink)}>POSTS</NavLink>

                    <NavLink to="/calculators" className={classNames(styles.navbarListItems, styles.navLink)}>
                        <FontAwesomeIcon icon={faCalculator} className={styles.navbarIcon}/>
                    </NavLink>

                    <NavLink to="/calendars" className={classNames(styles.navbarListItems, styles.navLink)}>
                        <FontAwesomeIcon icon={faCalendarDays} className={styles.navbarIcon}/>
                    </NavLink>

                    <NavLink to="/news" className={classNames(styles.navbarListItems, styles.navLink)}>
                        {
                            location.pathname === '/news' ?
                                <FontAwesomeIcon icon={faNewspaper} className={classnames(styles.navbarIcon, styles.newsIcon)} />
                            :
                                <FontAwesomeIcon icon={faNewspaper} className={classnames(styles.navbarIcon, styles.newsIcon)} beatFade />
                        }
                    </NavLink>

                    <NavLink to="/search" className={classNames(styles.navbarListItems, styles.navLink)}>
                        <FontAwesomeIcon icon={faMagnifyingGlass} className={styles.navbarIcon}/>
                    </NavLink>
                </div>
                <div className={styles.userPanel}>
                    {JWT ?
                        <div className={styles.authContainer}>
                            <NavLink to={`/users/${email}/personal-info`} className={classNames(styles.navbarListItems, styles.navLink)}>
                                <FontAwesomeIcon icon={faUser} className={styles.navbarIcon}/>
                            </NavLink>
                            <NavLink to="/logout" className={classNames(styles.navbarListItems, styles.navLink)}>
                                <FontAwesomeIcon icon={faRightFromBracket} className={styles.navbarIcon}/>
                            </NavLink>
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