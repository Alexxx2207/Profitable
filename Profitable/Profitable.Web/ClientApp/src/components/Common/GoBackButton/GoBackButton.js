import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowLeft } from "@fortawesome/free-solid-svg-icons";

import styles from "./GoBackButton.module.css";

export const GoBackButton = ({ link }) => {
    const navigate = useNavigate();

    const handleClick = (e) => {
        navigate(link);
    };

    return (
        <button onClick={handleClick} className={styles.goBackButton}>
            <FontAwesomeIcon icon={faArrowLeft} />
            Go Back
        </button>
    );
};
