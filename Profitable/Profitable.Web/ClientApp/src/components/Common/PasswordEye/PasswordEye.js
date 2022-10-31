import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";

import styles from "./PasswordEye.module.css";

export const PasswordEye = ({ setPasswordView, opened }) => {
    const passwordEyeClickHandler = (e) => {
        e.preventDefault();

        setPasswordView();
    };

    return opened ? (
        <FontAwesomeIcon
            className={styles.eyeContainer}
            onClick={passwordEyeClickHandler}
            icon={faEye}
        />
    ) : (
        <FontAwesomeIcon
            className={styles.eyeContainer}
            onClick={passwordEyeClickHandler}
            icon={faEyeSlash}
        />
    );
};
