import { useCallback, useEffect, useReducer } from 'react';
import { useParams } from 'react-router-dom';
import { PositionsRecordListWidget } from './PositionsRecordListWidget/PositionsRecordListWidget';

import { getPositionsorderByOptions, getUserPositions } from '../../../../services/positions/positionsService';
import { POSITIONS_RECORDS_DEFAULT_ORDER, POSITIONS_RECORDS_PAGE_COUNT } from '../../../../common/config';


import styles from './PositionsRecordListsList.module.css';

const initialState = {
    lists: [],
    page: 1,
    orderPositionsRecordsBySelected: POSITIONS_RECORDS_DEFAULT_ORDER,
    positionsRecordsOrderByOptions: []
};

const reducer = (state, action) => {
    switch (action.type) {
        case 'increasePage': {
            return {
                ...state,
                page: (state.page + 1)
            }
        }
        case 'loadOrderByOptions': {
            return {
                ...state,
                positionsRecordsOrderByOptions: action.payload
            }
        }
        case 'loadRecords': {
            return {
                ...state,
                lists: [...state.lists, ...action.payload]
            }
        } 
        case 'changeSelectedOrderOption': {
            return {
                ...state,
                orderPositionsRecordsBySelected: action.payload
            }
        } 
        default:
            return {
                ...state
            };
    }
};

export const PositionsRecordListsList = () => {

    const {searchedProfileEmail} = useParams();

    const [state, setState] = useReducer(reducer, initialState);

    useEffect(() => {
        getPositionsorderByOptions()
            .then(result => {
                setState({
                    type: 'loadOrderByOptions',
                    payload: result.types
                })
            })
    }, []);

    useEffect(() => {
        getUserPositions(searchedProfileEmail, 0, POSITIONS_RECORDS_PAGE_COUNT, POSITIONS_RECORDS_DEFAULT_ORDER)
        .then(result => {
            setState({
                type: 'loadRecords',
                payload: result
            });
        });
    }, [searchedProfileEmail]);

    const changeSelectedOrderOption = (e) => {
        setState({
            type: 'changeSelectedOrderOption',
            payload: e.target.value
        });

        getUserPositions(searchedProfileEmail, 0, POSITIONS_RECORDS_PAGE_COUNT, e.target.value)
            .then(result => {
                setState({
                    type: 'loadRecords',
                    payload: result
                });
            });
    }

    const loadRecords = useCallback((page, pageCount, orderPositionsRecordsBySelected) => {
        console.log(page);
        getUserPositions(searchedProfileEmail, page, pageCount, orderPositionsRecordsBySelected)
            .then(result => {
                console.log(result);
                setState({
                    type: 'loadRecords',
                    payload: result
                });
            });
    }, [searchedProfileEmail]);

    const handleScroll = useCallback((e) => {
        const scrollHeight = e.target.documentElement.scrollHeight;
        const currentHeight = e.target.documentElement.scrollTop + window.innerHeight;

        if (currentHeight >= scrollHeight - 1 || currentHeight === scrollHeight) {
            setState({
                type: 'increasePageCount'
            });
            loadRecords(state.page, POSITIONS_RECORDS_PAGE_COUNT, state.orderPositionsRecordsBySelected);
        }
    }, [loadRecords, state.orderPositionsRecordsBySelected, state.page]);

    useEffect(() => {
        document.addEventListener('scroll', handleScroll);

        return () => window.removeEventListener('scroll', handleScroll);
    }, [handleScroll])

    return (
        <div className={styles.listContainer}>
            <div className={styles.orderSelectorContainer}>
                <h5 className={styles.orderByHeader}>Order By</h5>
                <select
                    onChange={changeSelectedOrderOption}
                    value={state.orderPositionsRecordsBySelected}
                    className={styles.orderBySelector}>
                    {state.positionsRecordsOrderByOptions.map(option => 
                        <option value={option.split(' ').join('')}>{option}</option>
                    )}
                </select>
            </div>
            {state.lists.map(list => <PositionsRecordListWidget key={list.guid} list={list} />)}
        </div>
    );
}