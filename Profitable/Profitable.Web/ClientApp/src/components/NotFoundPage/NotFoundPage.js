import { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { MISSING_POST_GUID_ERROR_PAGE_PATH } from '../../common/config';

import styles from './NotFoundPage.module.css';

export const NotFoundPage = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const [redirectButtonData, setRedirectButtonData] = useState({
        textContent: 'Home Page',
        url: '/'
    });

    useEffect(() => {
        if(location.pathname === MISSING_POST_GUID_ERROR_PAGE_PATH) {
            setRedirectButtonData({
                textContent: 'Go To Posts List',
                url: '/posts'
            })
        }
    }, []);

    return (
        <div className={styles.missingPageContainer}>
            <div className={styles.mainContent}>
                <h1 className={styles.logo404}>404</h1>
                <h5 className={styles.teaseContainer}>Your market research has gone to another level
                    <br />Therefore, you have entered an area unknown to us
                    <br />You can start or resume researching and analysing by clicking the button below
                </h5>
                <button onClick={() => navigate(redirectButtonData.url)} className={styles.homeButton}>
                    {redirectButtonData.textContent}
                </button>
            </div>
        </div>
    );
}