import styles from "./SavePositionsRecordListWidget.module.css";

export const SavePositionsRecordListWidget = ({list, handleSave}) => {   
    return (
        <div className={styles.widgetContainer}>
            <div>
                <h2 className={styles.recordNameHeader}>{list.name}</h2>
                <h5>Recorded Positions: {list.positionsCount}</h5>
                <h5>Financial Instruments: {list.instrumentGroup}</h5>
                <h6>Last Updated: {list.lastUpdated}</h6>
            </div>
            <div className={styles.savePositionsRecordButtonContainer}>
                <button className={styles.savePositionsRecordButton} onClick={() => handleSave(list)}>
                    Save
                </button>
            </div>
        </div>
    );
}