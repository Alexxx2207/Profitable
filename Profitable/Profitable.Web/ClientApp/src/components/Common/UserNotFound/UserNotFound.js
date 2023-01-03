import { useNavigate } from "react-router-dom";

import styles from "./UserNotFound.module.css";

export const UserNotFound = () => {
    const navigate = useNavigate();

    return (
        <div className={styles.missingPageContainer}>
            <div className={styles.mainContent}>
                <h1 className={styles.logo404}>404</h1>
                <h5 className={styles.teaseContainer}>
                    User was not found!
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
