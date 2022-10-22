import { useEffect, useReducer } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getPositionsFromRecord } from "../../../../../services/positions/positionsService";
import { GoBackButton } from "../../../../GoBackButton/GoBackButton";
import { Line } from "react-chartjs-2";
import { calculateAcculativePositions } from '../../../../../services/positions/positionsService';
import { CategoryScale, LineElement, PointElement, LinearScale, Chart, Tooltip, Filler } from "chart.js";

import styles from './FuturesRecordDetails.module.css';
import { ShortDirectionName } from "../../../../../common/config";


Chart.register([CategoryScale, LineElement, PointElement, LinearScale, Tooltip, Filler]);

const reducer = (state, action) => {
    switch (action.type) {
        case 'loadPositions':
            var accumulatedPositions = [...calculateAcculativePositions([0, ...[...action.payload].map(position => position.positionPAndL).reverse()])];

            return {
                ...state,
                positions: [...action.payload],
                accumulatedPandLPositions: accumulatedPositions,
                overallProfitLoss: Number(accumulatedPositions[accumulatedPositions.length-1]),
            };
        case 'setDefaultDate': 
            return {
                ...state,
                selectedAfterDateFilter: action.payload
            }
        default:
            return {
                ...state
            };
    }
}

export const FuturesRecordDetails = () => {

    const [state, setState] = useReducer(reducer, {
        positions: [],
        accumulatedPandLPositions: [],
        overallProfitLoss: 0,
        selectedAfterDateFilter: '',
    });

    const { recordGuid, searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    useEffect(() => {
        setTimeout(() => {
            setState({type: 'refresh'});
        }, 150);
    }, []);

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
                <Line
                data={{
                        labels: [0, ...state.positions.map(position => position.positionAddedOn).reverse()],
                        datasets: [
                        {
                            fill: 'origin',
                            data: [...state.accumulatedPandLPositions],
                            pointBackgroundColor: 'white',
                            borderColor: 'white',
                            pointRadius: 5,
                            pointHoverRadius: 8,
                        },
                    ],
                    }
                }

                options={
                    {
                        indexAxis: 'x',
                        maintainAspectRatio: false,
                        responsive: true,
                        scales: {
                            x: {
                                grid: {
                                    borderColor: 'white'
                                },
                                display: false
                            },
                            y: {
                                suggestedMin: -Math.max(...state.accumulatedPandLPositions),
                                suggestedMax: Math.max(...state.accumulatedPandLPositions),
                                grid: {
                                    borderColor: 'white',
                                    color: 'rgb(255,255,255, 0.2)'
                                },
                                ticks: {
                                    beginAtZero: true,
                                    font: {
                                        size: 13,
                                    },
                                    callback: function(value) {
                                        if(Number(value) < 0) {
                                            return ` -$${Math.abs(Number(value)).toFixed(2)}`;
                                        }
                                        return ` $${Math.abs(Number(value)).toFixed(2)}`;
                                    },
                                    color: 'white',
                                }
                            },
                        },
                        plugins: {
                            filler: {
                                propagate: true
                            },
                            tooltip: {
                                backgroundColor: 'black',
                                titleFont: {
                                    size: 15,
                                    weight: 500,
                                },
                                bodyFont: {
                                    size: 15,
                                    weight: 700,
                                },
                                callbacks: {
                                    label: (context) => {
                                        if(Number(context.parsed.y) < 0) {
                                            return ` -$${Math.abs(Number(context.parsed.y)).toFixed(2)}`;
                                        }
                                        return ` $${Math.abs(Number(context.parsed.y)).toFixed(2)}`;
                                    }
                                }
                            },
                        }
                    }
                } 
                
                plugins={[{
                    beforeDraw: function (state, options) {
                        const chart = state.$context.chart;
                        const dataset = state.data.datasets[0];
                        const yScale = state.scales.y;
                        const yPos = yScale.getPixelForValue(0);

                        const gradientFill = chart.ctx.createLinearGradient(0, 0, 0, chart.height);
                        gradientFill.addColorStop(0, 'green');
                        gradientFill.addColorStop(yPos / chart.height, 'rgb(0, 255, 0, 0.1)');
                        gradientFill.addColorStop(yPos / chart.height, 'rgb(255, 0, 0, 0.1)');
                        gradientFill.addColorStop(1, 'red');
                        
                        dataset.backgroundColor = gradientFill;
                    }
                }]}/>
            </div>

            <div className={styles.addPositionButtonContainer}>
                <button className={styles.addPositionButton} onClick={addPositionButtonClickHandler}>+Add Position</button>
            </div>
            <div className={styles.overallProfiltLossHeading}>
                <h3>Overall Profit/Loss: <span className={state.overallProfitLoss < 0 ? styles.textColorRed : styles.textColorGreen}>
                    ${state.overallProfitLoss.toFixed(2)}
                    </span>
                </h3>
            </div>
            <table className={styles.positionsTable}>
                <thead>
                    <tr>
                        <th></th>
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
                    {state.positions.reverse().map((position, index) => <tr key={index}>
                        <td>
                            {index+1}
                        </td>
                        <td>
                            {position.positionAddedOn}
                        </td>
                        <td>
                            {position.contractName}
                        </td>
                            {position.direction.localeCompare(ShortDirectionName) === 0 ?
                                <td className={styles.textColorRed}>
                                    {position.direction}
                                </td>
                            :
                                <td className={styles.textColorGreen}>
                                    {position.direction}
                                </td>
                            }
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
                        {position.positionPAndL < 0 ?
                            <td className={styles.textColorRed}>
                                {Number(position.positionPAndL).toFixed(2)}
                            </td>
                        :
                            <td className={styles.textColorGreen}>
                                {Number(position.positionPAndL).toFixed(2)}
                            </td>
                        }
                    </tr>)}
                </tbody>
            </table>
        </div>
    );
}