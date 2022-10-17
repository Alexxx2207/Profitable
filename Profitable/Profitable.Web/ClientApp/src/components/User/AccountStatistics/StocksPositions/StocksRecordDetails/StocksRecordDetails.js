import { useEffect, useReducer } from "react";
import { useParams } from "react-router-dom";
import { getPositionsFromRecord } from "../../../../../services/positions/positionsService";


import styles from './StocksRecordDetails.module.css';

const reducer = (state, action) => {
    switch (action.type) {
        case 'loadPositions':
            return {
                ...state,

            };
        case 'setDefaultDate': 
            return {
                ...state,
                selectedAfterDateFilter: action.payload
            }
        default:
            break;
    }
}

export const StocksRecordDetails = () => {

    const [state, setState] = useReducer(reducer, {
        positions: [],
        selectedAfterDateFilter: ''
    });

    const { recordGuid } = useParams();

    useEffect(() => {
        var oneYearAgoFromNow = new Date();
        oneYearAgoFromNow.setFullYear(oneYearAgoFromNow.getFullYear() - 1);

        setState({
            type: 'setDefaultDate',
            payload: oneYearAgoFromNow
        });
        getPositionsFromRecord(recordGuid, oneYearAgoFromNow.toJSON())
            .then(result => 
                setState({
                    type: 'loadPositions',
                    payload: result
                })
            )
    }, [recordGuid]);

    return (
        <div className={styles.recordDetailsContainer}>
            <table>
                <thead>
                    <th>
                        Date Added
                    </th>
                    <th>
                        Contract/Instrument
                    </th>
                    <th>
                        Direction
                    </th>
                    <th>
                        Entry Price
                    </th>
                    <th>
                        Exit Price
                    </th>
                    <th>
                        Quantity
                    </th>
                    <th>
                        Tick Size
                    </th>
                    <th>
                        Tick Value
                    </th>
                    <th>
                        P&L
                    </th>
                </thead>
                <tbody>
                    {state.positions.map(position => <tr>
                        <td>
                            {position.positionAddedOn}
                        </td>
                        <td>
                            {position.contractName}
                        </td>
                        <td>
                            {position.direction}
                        </td>
                        <td>
                            {position.entryPrice}
                        </td>
                        <td>
                            {position.exitPrice}
                        </td>
                        <td>
                            {position.quantity}
                        </td>
                        <td>
                            {position.tickSize}
                        </td>
                        <td>
                            {position.tickValue}
                        </td>
                        <td>
                            {position.positionPAndL}
                        </td>
                    </tr>)}
                </tbody>
            </table>
        </div>
    );
}