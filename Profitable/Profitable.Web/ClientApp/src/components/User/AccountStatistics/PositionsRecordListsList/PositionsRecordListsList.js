import { useEffect, useReducer } from 'react';
import { PositionsRecordListWidget } from './PositionsRecordListWidget/PositionsRecordListWidget';

import { getUserPositions } from '../../../../services/positions/positionsService';
import { POSITIONS_RECORDS_DEFAULT_ORDER, POSITIONS_RECORDS_PAGE_COUNT } from '../../../../common/config';


import styles from './PositionsRecordListsList.module.css';
import { useParams } from 'react-router-dom';

const initialState = {
    lists: [],
    page: 1,
    orderPositionsRecordsBy: POSITIONS_RECORDS_DEFAULT_ORDER
};

const reducer = (state, action) => {
    switch (action.type) {
        case 'increasePage': {
            return {
                ...state,
                page: (state.page + 1)
            }
        }
        case 'loadRecords': {
            return {
                ...state,
                lists: action.payload
            }
        } 
        default:
            break;
    }
};

export const PositionsRecordListsList = () => {

    const {searchedProfileEmail} = useParams();

    const [state, setState] = useReducer(reducer, initialState);

    useEffect(() => {
        getUserPositions(searchedProfileEmail, 0, POSITIONS_RECORDS_PAGE_COUNT, POSITIONS_RECORDS_DEFAULT_ORDER)
        .then(result => {
            setState({
                type: 'loadRecords',
                payload: result
            });
        })
    }, [searchedProfileEmail]);

    return (
        <div className={styles.listContainer}>
            {state.lists.map(list => <PositionsRecordListWidget list={list} />)}
        </div>
    );
}