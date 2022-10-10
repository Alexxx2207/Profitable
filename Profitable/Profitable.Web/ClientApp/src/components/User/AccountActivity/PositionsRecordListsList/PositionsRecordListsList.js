

import styles from './PositionsRecordListsList.module.css';
import { PositionsRecordListWidget } from './PositionsRecordListWidget/PositionsRecordListWidget';

export const PositionsRecordListsList = ({lists}) => {
    return (
        <div className={styles.listContainer}>
            {lists.map(list => <PositionsRecordListWidget list={list} />)}
        </div>
    );
}