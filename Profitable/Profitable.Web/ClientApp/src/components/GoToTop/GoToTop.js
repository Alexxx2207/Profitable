import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faArrowUp } from "@fortawesome/free-solid-svg-icons";

import styles from "./GoToTop.module.css";
import { useEffect, useState } from "react";

export const GoToTop = () => {
    const [showGoToTop, setShowGoToTop] = useState(false);

    const handleScroll = () => {
        if (window.pageYOffset > window.screenY) {
            setShowGoToTop(true);
        } else {
            setShowGoToTop(false);
        }
    };

    useEffect(() => {
        document.addEventListener("scroll", handleScroll);

        return () => {
            document.removeEventListener("scroll", handleScroll);
        };
    }, []);

    const handleClick = () => {
        window.scrollTo({
            top: 0,
            behavior: "smooth",
        });
    };

    return (
        <div>
            {showGoToTop ? (
                <button onClick={handleClick} className={styles.goToTopButton}>
                    <FontAwesomeIcon icon={faArrowUp} />
                </button>
            ) : (
                ""
            )}
        </div>
    );
};
