import { useEffect, useReducer, useContext, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { MessageBoxContext } from "../../../../../contexts/MessageBoxContext";
import { AuthContext } from "../../../../../contexts/AuthContext";

import {
    deletePosition,
    getPositionsFromRecord,
    calculateAcculativePositions,
} from "../../../../../services/positions/stocksPositionsService";
import { getUserEmailFromJWT } from "../../../../../services/users/usersService";

import { GoBackButton } from "../../../../Common/GoBackButton/GoBackButton";
import { Line } from "react-chartjs-2";
import {
    CategoryScale,
    LineElement,
    PointElement,
    LinearScale,
    Chart,
    Tooltip,
    Filler,
} from "chart.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faWrench } from "@fortawesome/free-solid-svg-icons";

import styles from "./StocksRecordDetails.module.css";

Chart.register([CategoryScale, LineElement, PointElement, LinearScale, Tooltip, Filler]);

const reducer = (state, action) => {
    switch (action.type) {
        case "loadPositions":
            var accumulatedPositions = [
                ...calculateAcculativePositions([
                    0,
                    ...[...action.payload].map((position) => position.realizedProfitAndLoss),
                ]),
            ];

            return {
                ...state,
                positions: [...action.payload],
                accumulatedPandLPositions: accumulatedPositions,
                overallProfitLoss: Number(accumulatedPositions[accumulatedPositions.length - 1]),
            };
        case "setDefaultDate":
            return {
                ...state,
                selectedAfterDateFilter: action.payload,
            };
        default:
            return {
                ...state,
            };
    }
};

export const StocksRecordDetails = () => {
    const [state, setState] = useReducer(reducer, {
        positions: [],
        accumulatedPandLPositions: [],
        overallProfitLoss: 0,
        selectedAfterDateFilter: "",
    });

    const [loggedInUserEmail, setLoggedInUserEmail] = useState("");

    const { recordGuid, searchedProfileEmail } = useParams();

    const navigate = useNavigate();

    const location = useLocation();

    const { setMessageBoxSettings } = useContext(MessageBoxContext);

    const { JWT } = useContext(AuthContext);

    useEffect(() => {
        setTimeout(() => {
            setState({ type: "refresh" });
        }, 150);
        getUserEmailFromJWT(JWT).then((email) => setLoggedInUserEmail(email));
    }, []);

    useEffect(() => {
        var oneYearAgoFromNow = new Date();
        oneYearAgoFromNow.setFullYear(oneYearAgoFromNow.getFullYear() - 1);
        var pathnameArray = location.pathname.split("/");

        setState({
            type: "setDefaultDate",
            payload: oneYearAgoFromNow,
        });
        getPositionsFromRecord(
            recordGuid,
            pathnameArray[pathnameArray.length - 2],
            oneYearAgoFromNow.toJSON()
        ).then((result) => {
            setState({
                type: "loadPositions",
                payload: result,
            });
        });
    }, [recordGuid]);

    const deletePositionOnClickHandler = (positionGuid) => {
        deletePosition(JWT, recordGuid, positionGuid)
            .then(() => {
                setMessageBoxSettings("The position was deleted successfully!", true);

                setTimeout(() => {
                    window.location.reload();
                }, 1000);
            })
            .catch((error) => {
                if (error.message === "401") {
                    setMessageBoxSettings("Authenticate before delete this position!", true);
                    navigate("/login");
                }
            });
    };

    const addPositionButtonClickHandler = (e) => {
        navigate(
            `/users/${searchedProfileEmail}/positions-records/stocks/${recordGuid}/create-position`
        );
    };

    const changePositionOnClickHandler = (positionGuid) => {
        navigate(
            `/users/${searchedProfileEmail}/positions-records/stocks/${recordGuid}/change-position/${positionGuid}`
        );
    };

    return (
        <div className={styles.recordDetailsContainer}>
            <div>
                <GoBackButton link={`/users/${searchedProfileEmail}/account-statistics`} />
            </div>

            <div className={styles.chartContainer}>
                <Line
                    data={{
                        labels: [0, ...state.positions.map((position) => position.positionAddedOn)],
                        datasets: [
                            {
                                fill: "origin",
                                data: [...state.accumulatedPandLPositions],
                                pointBackgroundColor: "white",
                                borderColor: "white",
                                pointRadius: 5,
                                pointHoverRadius: 8,
                            },
                        ],
                    }}
                    options={{
                        indexAxis: "x",
                        maintainAspectRatio: false,
                        responsive: true,
                        scales: {
                            x: {
                                grid: {
                                    borderColor: "white",
                                },
                                display: false,
                            },
                            y: {
                                grid: {
                                    borderColor: "white",
                                    color: "rgb(255,255,255, 0.2)",
                                },
                                ticks: {
                                    beginAtZero: true,
                                    font: {
                                        size: 13,
                                    },
                                    callback: function (value) {
                                        if (Number(value) < 0) {
                                            return ` -$${Math.abs(Number(value)).toFixed(2)}`;
                                        }
                                        return ` $${Math.abs(Number(value)).toFixed(2)}`;
                                    },
                                    color: "white",
                                },
                            },
                        },
                        plugins: {
                            filler: {
                                propagate: true,
                            },
                            tooltip: {
                                backgroundColor: "black",
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
                                        if (Number(context.parsed.y) < 0) {
                                            return ` -$${Math.abs(Number(context.parsed.y)).toFixed(
                                                2
                                            )}`;
                                        }
                                        return ` $${Math.abs(Number(context.parsed.y)).toFixed(2)}`;
                                    },
                                },
                            },
                        },
                    }}
                    plugins={[
                        {
                            beforeDraw: function (state, options) {
                                const chart = state.$context.chart;
                                const dataset = state.data.datasets[0];
                                const yScale = state.scales.y;
                                const yPos = yScale.getPixelForValue(0);

                                const gradientFill = chart.ctx.createLinearGradient(
                                    0,
                                    0,
                                    0,
                                    chart.height
                                );
                                gradientFill.addColorStop(0, "green");
                                gradientFill.addColorStop(
                                    yPos / chart.height,
                                    "rgb(0, 255, 0, 0.1)"
                                );
                                gradientFill.addColorStop(
                                    yPos / chart.height,
                                    "rgb(255, 0, 0, 0.1)"
                                );
                                gradientFill.addColorStop(1, "red");

                                dataset.backgroundColor = gradientFill;
                            },
                        },
                    ]}
                />
            </div>

            {searchedProfileEmail.localeCompare(loggedInUserEmail) === 0 ? (
                <div className={styles.addPositionButtonContainer}>
                    <button
                        className={styles.addPositionButton}
                        onClick={addPositionButtonClickHandler}
                    >
                        +Add Position
                    </button>
                </div>
            ) : (
                ""
            )}

            <div className={styles.overallProfiltLossHeading}>
                <h3>
                    Overall Profit/Loss:{" "}
                    <span
                        className={
                            state.overallProfitLoss < 0
                                ? styles.textColorRed
                                : styles.textColorGreen
                        }
                    >
                        ${state.overallProfitLoss.toFixed(2)}
                    </span>
                </h3>
            </div>

            <table className={styles.positionsTable}>
                <thead>
                    <tr>
                        <th></th>
                        <th>Date Added</th>
                        <th>Stock Name</th>
                        <th>Entry Price</th>
                        <th>Exit Price</th>
                        <th>Quantity</th>
                        <th>P/L ($)</th>

                        {searchedProfileEmail.localeCompare(loggedInUserEmail) === 0 ? (
                            <>
                                <th>Delete</th>
                                <th>Update</th>
                            </>
                        ) : (
                            ""
                        )}
                    </tr>
                </thead>
                <tbody>
                    {state.positions.reverse().map((position, index) => (
                        <tr key={index}>
                            <td>{index + 1}</td>
                            <td>{position.positionAddedOn}</td>
                            <td>{position.name}</td>
                            <td>{position.entryPrice}</td>
                            <td>{position.exitPrice}</td>
                            <td>{position.quantitySize}</td>
                            {position.realizedProfitAndLoss < 0 ? (
                                <td className={styles.textColorRed}>
                                    {Number(position.realizedProfitAndLoss).toFixed(2)}
                                </td>
                            ) : (
                                <td className={styles.textColorGreen}>
                                    {Number(position.realizedProfitAndLoss).toFixed(2)}
                                </td>
                            )}
                            {searchedProfileEmail.localeCompare(loggedInUserEmail) === 0 ? (
                                <>
                                    <td>
                                        <button
                                            className={styles.buttonPositionDelete}
                                            onClick={deletePositionOnClickHandler.bind(
                                                null,
                                                position.guid
                                            )}
                                        >
                                            <FontAwesomeIcon icon={faTrash} />
                                        </button>
                                    </td>
                                    <td>
                                        <button
                                            className={styles.buttonPositionChange}
                                            onClick={changePositionOnClickHandler.bind(
                                                null,
                                                position.guid
                                            )}
                                        >
                                            <FontAwesomeIcon icon={faWrench} />
                                        </button>
                                    </td>
                                </>
                            ) : (
                                ""
                            )}
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};
