import { PositionsRecordListWidget } from './PositionsRecordListWidget/PositionsRecordListWidget';

import styles from './PositionsRecordListsList.module.css';



export const PositionsRecordListsList = ({records}) => {

    return (
        <div className={styles.listContainer}>
            {records.map((list, index) => <PositionsRecordListWidget key={index} list={list} />)}
        </div>
    );
}