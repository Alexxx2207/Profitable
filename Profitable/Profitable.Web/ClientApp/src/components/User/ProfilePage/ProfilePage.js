import { NavLink, Outlet, useLocation  } from 'react-router-dom';
import React, { useEffect, useState } from 'react';

import styles from './ProfilePage.module.css';
import { GoToTop } from '../../GoToTop/GoToTop';

export const ProfilePage = () => {

    const location = useLocation();

    const [url, setUrl] = useState('');

    useEffect(() => {
      setUrl(location.pathname);
    }, [location]);

    return (
        <div className={styles.profilePageContainer}>
            <nav className={styles.profilePageNavbarContainer}>
                <NavLink to={'personal-info'} className={(url.includes('personal-info') ? styles.active : "")}>
                    Personal
                </NavLink>
                <NavLink to={'account-statistics'} className={(url.includes('account-statistics') ? styles.active : "")}>
                    Statistics
                </NavLink>
                <NavLink to={'account-activity'} className={(url.includes('account-activity') ? styles.active : "")}>
                    Activity
                </NavLink>
            </nav>
            <Outlet />
            <GoToTop />
        </div>
    );
}