

import styles from "./ShowMoreButton.module.css";

export const ShowMoreButton = ({showShowMore, entity, handler}) => {
    return (
        <div>
            {showShowMore ? (
                    <div className={styles.loadMoreContainer}>
                        <h4 className={styles.loadMoreButton} onClick={handler}>
                            Show More {entity}
                        </h4>
                    </div>
                ) : (
                    <></>
                )}
        </div>
    );
}