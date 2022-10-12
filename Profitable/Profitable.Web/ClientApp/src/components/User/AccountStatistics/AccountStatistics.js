import { PositionsRecordListsList } from './PositionsRecordListsList/PositionsRecordListsList';
import { useNavigate, useParams } from 'react-router-dom';


import styles from './AccountStatistics.module.css';

export const AccountStatistics = () => {

    const navigate = useNavigate();

    const {searchedProfileEmail} = useParams();

    const handleAddRecordButtonClick = () => {
        navigate(`/${searchedProfileEmail}/positions-record/create`);
    }

    return(
        <div>
            <div className={styles.recordsListHeader}>
                <h1>Records List</h1>

                <button onClick={handleAddRecordButtonClick} className={styles.addRecordButton}>
                    + Add Record
                </button>
            </div>
            
            <PositionsRecordListsList />
        </div>
    );
};