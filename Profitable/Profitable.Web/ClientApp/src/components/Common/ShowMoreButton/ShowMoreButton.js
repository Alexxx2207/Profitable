

import styles from "./ShowMoreButton.module.css";

export const ShowMoreButton = ({showShowMore, entity, handler}) => {
    return (
        <div>
            {showShowMore ? (
                    <div className={styles.loadMoreContainer}>
                        <h5 className={styles.loadMoreButton} onClick={handler}>
                            Show More {entity}
                        </h5>
                    </div>
                ) : (
                    <></>
                )}
        </div>
    );
}