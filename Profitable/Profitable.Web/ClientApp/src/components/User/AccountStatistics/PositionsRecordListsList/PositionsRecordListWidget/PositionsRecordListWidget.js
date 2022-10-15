import { useNavigate } from 'react-router-dom';
import styles from './PositionsRecordListWidget.module.css';

export const PositionsRecordListWidget = ({list}) => {

    const navigate = useNavigate();

    const handleOpen = () => {
        navigate(`/positions-records/${list.guid}`);
    }
    return (
        <div className={styles.widgetContainer}>
            <div>
                <h1 className={styles.recordNameHeader}>{list.name}</h1>
                <h5>Recorded Positions: {list.positionsCount}</h5>
                <h6>Last Updated: {list.lastUpdated}</h6>
            </div>
            <div className={styles.openPositionsRecordButtonContainer}>
                <button className={styles.openPositionsRecordButton} onClick={() => handleOpen()}>Open</button>
            </div>
        </div>
    );
}