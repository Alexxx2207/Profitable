import { PositionsRecordListWidget } from "./PositionsRecordListWidget/PositionsRecordListWidget";

import styles from "./PositionsRecordListsList.module.css";

export const PositionsRecordListsList = ({ records, showOwnerActionButtons }) => {
    return (
        <div className={styles.listContainer}>
            {records.length > 0 ? (
                records.map((list) => (
                    <PositionsRecordListWidget
                        key={list.guid}
                        list={list}
                        showOwnerActionButtons={showOwnerActionButtons}
                    />
                ))
            ) : (
                <h2 className={styles.noRecordsHeader}>No Records Made Yet</h2>
            )}
        </div>
    );
};
