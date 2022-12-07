import { useState } from "react";
import { useNavigate } from "react-router-dom";

import styles from "./NotFoundPage.module.css";

export const NotFoundPage = () => {
    const navigate = useNavigate();

    return (
        <div className={styles.missingPageContainer}>
            <div className={styles.mainContent}>
                <h1 className={styles.logo404}>404</h1>
                <h5 className={styles.teaseContainer}>
                    Your market research has gone to another level
                    <br />
                    Therefore, you have entered an area unknown to us
                    <br />
                    You can start or resume researching and analysing by clicking the button below
                </h5>
                <button
                    onClick={() => navigate("/")}
                    className={styles.homeButton}
                >
                    Home Page
                </button>
            </div>
        </div>
    );
};
