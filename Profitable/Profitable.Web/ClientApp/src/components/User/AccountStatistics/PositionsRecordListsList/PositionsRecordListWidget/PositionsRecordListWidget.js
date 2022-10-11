import styles from './PositionsRecordListWidget.module.css';

export const PositionsRecordListWidget = ({list}) => {
    return (
        <div className={styles.widgetContainer}>
            <div>
                <h1>{list.Name}</h1>
                <h5>Recorded Positions: {list.positionsCount}</h5>
                <h6>Created On: {list.createdOn}</h6>
            </div>
        </div>
    );
}