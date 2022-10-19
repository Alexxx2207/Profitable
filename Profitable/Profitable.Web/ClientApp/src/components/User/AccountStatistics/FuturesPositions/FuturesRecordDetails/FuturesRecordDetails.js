import { useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getPositionsFromRecord } from "../../../../../services/positions/positionsService";
import { GoBackButton } from "../../../../GoBackButton/GoBackButton";
import { Line } from "react-chartjs-2";
import { calculateAcculativePositions } from '../../../../../services/positions/positionsService';
import { CategoryScale, LineElement, PointElement, LinearScale, Title, Chart } from "chart.js";

import styles from './FuturesRecordDetails.module.css';


Chart.register(CategoryScale, LineElement, PointElement, LinearScale, Title);

const reducer = (state, action) => {
    switch (action.type) {
        case 'loadPositions':
            return {
                ...state,
                positions: [...action.payload]
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



export const FuturesRecordDetails = () => {

    const [state, setState] = useReducer(reducer, {
        positions: [],
        selectedAfterDateFilter: ''
    });

    const { recordGuid, searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    useEffect(() => {
        var oneYearAgoFromNow = new Date();
        oneYearAgoFromNow.setFullYear(oneYearAgoFromNow.getFullYear() - 1);

        setState({
            type: 'setDefaultDate',
            payload: oneYearAgoFromNow
        });
        getPositionsFromRecord(recordGuid, oneYearAgoFromNow.toJSON())
            .then(result => {
                setState({
                    type: 'loadPositions',
                    payload: result
                })
            });
    }, [recordGuid]);

    const addPositionButtonClickHandler = (e) => {
        navigate(`/users/${searchedProfileEmail}/positions-records/futures/${recordGuid}/create-position`);
    }
    
    return (
        <div className={styles.recordDetailsContainer}>
            <div>
                <GoBackButton link={`/users/${searchedProfileEmail}/account-statistics`} />
            </div>

            <div className={styles.chartContainer}>
                <Line data={
                     {
                        labels: [0, ...state.positions.map(position => position.positionAddedOn).reverse()],
                        datasets: [{
                            label: 'Performance',
                            data:  calculateAcculativePositions([0,...state.positions.map(position => position.positionPAndL).reverse()]),
                            backgroundColor: 'white',
                            borderColor: 'white',
                            pointRadius: 5,
                            pointHoverRadius: 10
                        }],
                    }
                }
                options={
                    {
                        maintainAspectRatio: false,
                        responsive: true,
                        scales: {
                            xAxes: {
                                grid: {
                                    borderColor: 'white'
                                    
                                },
                                display: false
                            },
                            y: {
                                grid: {
                                    borderColor: 'white',
                                    color: 'rgb(255,255,255, 0.2)'
                                },
                                ticks: {
                                    callback: function(value) {
                                        return '$' + value;
                                    },
                                    color: 'white',
                                }
                            },
                        }
                    }
                } />
            </div>

            <div className={styles.addPositionButtonContainer}>
                <button className={styles.addPositionButton} onClick={addPositionButtonClickHandler}>+Add Position</button>
            </div>
            <table className={styles.positionsTable}>
                <thead>
                    <tr>
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
                            P/L ($)
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {state.positions.map((position, index) => <tr key={index}>
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