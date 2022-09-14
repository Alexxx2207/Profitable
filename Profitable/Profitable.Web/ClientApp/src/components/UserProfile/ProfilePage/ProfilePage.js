import { NavLink, Outlet, useLocation  } from 'react-router-dom';
import React, { useEffect, useState } from 'react';

import styles from './ProfilePage.module.css';
import { skipPartiallyEmittedExpressions } from 'typescript';

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
                    Personal Info
                </NavLink>
                <NavLink to={'account-statistics'} className={(url.includes('account-statistics') ? styles.active : "")}>Account Statistics</NavLink>
            </nav>
            <Outlet />
        </div>
    );
}