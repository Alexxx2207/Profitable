import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { deleteUserPositionsRecord } from '../../../../../services/positions/positionsService';
import { AuthContext } from '../../../../../contexts/AuthContext';


import styles from './PositionsRecordListWidget.module.css';
import { useParams } from 'react-router-dom';

export const PositionsRecordListWidget = ({list, showOwnerActionButtons}) => {

    const navigate = useNavigate();

    const { searchedProfileEmail } = useParams();

    const { JWT } = useContext(AuthContext);

    const handleOpen = () => {
        navigate(`/positions-records/${list.guid}`);
    };
    
    const handleChange = () => {
        navigate(`/users/${searchedProfileEmail}/positions-records/${list.guid}/change`);
    };

    const handleDelete = () => {
        deleteUserPositionsRecord(JWT, list.guid)
            .then(
                window.location.reload()
            );
    };

    return (
        <div className={styles.widgetContainer}>
            <div>
                <h2 className={styles.recordNameHeader}>{list.name}</h2>
                <h5>Recorded Positions: {list.positionsCount}</h5>
                <h5>Finantial Instruments: {list.instrumentGroup}</h5>
                <h6>Last Updated: {list.lastUpdated}</h6>
            </div>
            <div className={styles.openPositionsRecordButtonContainer}>
                <button className={styles.openPositionsRecordButton} onClick={() => handleOpen()}>Open</button>
                {showOwnerActionButtons ? 
                    <>
                        <button className={styles.changeNamePositionsRecordButton} onClick={() => handleChange()}>Change Name</button>
                        <button className={styles.deletePositionsRecordButton} onClick={() => handleDelete()}>Delete</button>
                    </>
                :
                    ''
                }
            </div>
        </div>
    );
}