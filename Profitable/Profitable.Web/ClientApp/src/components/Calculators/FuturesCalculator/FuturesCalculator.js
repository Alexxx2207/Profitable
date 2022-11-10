import { useEffect, useReducer } from "react";
import { LongDirectionValue, ShortDirectionValue } from "../../../common/config";

import { loadFuturesContracts, CalculatePosition } from "../../../services/futures/futuresService";

import styles from "./FuturesCalculator.module.css";

const initialState = {
    allFutures: [],
    chosenFutures: {
        name: "",
        tickSize: 0,
        tickValue: 0,
    },
    position: {
        directionBullish: true,
        entryPrice: 0,
        exitPrice: 0,
        numberOfContracts: 0,
    },
    answer: {
        ticks: 0,
        USDValue: 0,
    },
};

function futuresReducer(state, action) {
    switch (action.type) {
        case "loadAllFutures":
            return {
                ...state,
                allFutures: action.allFutures,
                chosenFutures: action.allFutures[0],
            };
        case "selectContractType":
            return {
                ...state,
                chosenFutures: state.allFutures[action.index],
            };
        case "selectContractDirection":
            return {
                ...state,
                position: {
                    ...state.position,
                    directionBullish: action.directionBullish,
                },
            };
        case "selectContractEntryPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryPrice: action.entryPrice,
                },
            };
        case "selectContractExitPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitPrice: action.exitPrice,
                },
            };
        case "selectContractsCount":
            if (action.numberOfContracts >= 0) {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfContracts: action.numberOfContracts,
                    },
                };
            } else {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfContracts: 0,
                    },
                };
            }
        case "setResult":
            return {
                ...state,
                answer: {
                    ticks: action.ticks,
                    USDValue: action.USDValue,
                },
            };
        default:
            break;
    }
}

export const FuturesCalculator = () => {
    const [futures, dispatch] = useReducer(futuresReducer, initialState);

    useEffect(() => {
        loadFuturesContracts().then((futuresContracts) =>
            dispatch({ type: "loadAllFutures", allFutures: futuresContracts })
        );
    }, []);

    const OnChangeContractType = (value) => {
        dispatch({ type: "selectContractType", index: value });
    };

    const onBullishDirectionButtonClick = (e) => {
        dispatch({ type: "selectContractDirection", directionBullish: true });
    };

    const onBearishDirectionButtonClick = (e) => {
        dispatch({ type: "selectContractDirection", directionBullish: false });
    };

    const entryPriceOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectContractEntryPrice", entryPrice: e.target.value });
    };

    const exitPriceOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectContractExitPrice", exitPrice: e.target.value });
    };

    const contractsNumberOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectContractsCount", numberOfContracts: e.target.value });
    };

    const CalculatePLResult = (e) => {
        e.preventDefault();

        CalculatePosition(
            futures.position.directionBullish ? LongDirectionValue : ShortDirectionValue,
            Number(futures.position.entryPrice),
            Number(futures.position.exitPrice),
            Number(futures.position.numberOfContracts),
            futures.chosenFutures.tickSize,
            futures.chosenFutures.tickValue
        ).then((result) => {
            console.log(result);
            dispatch({ type: "setResult", ticks: result.ticks, USDValue: result.profitLoss });
        });
    };

    return (
        <div className={styles.futuresCalculatorWrapper}>
            <h2 className={styles.futuresCalculatorHeading}>Futures Calculator</h2>
            <form className={styles.futuresCalculatorForm}>
                <div className={styles.contractTypeContainer}>
                    <h4 className={styles.contractTypeHeading}>Select Contract</h4>
                    <select
                        className={styles.selectContract}
                        onChange={(e) => OnChangeContractType(e.target.value)}
                        select={futures.chosenFutures.name}
                    >
                        {futures.allFutures.map((futureContract, index) => (
                            <option
                                className={styles.selectContractOption}
                                key={futureContract.guid}
                                value={index}
                            >
                                {futureContract.name}
                            </option>
                        ))}
                    </select>
                </div>
                <div className={styles.futuresInformation}>
                    <div className={styles.positionInformationContainer}>
                        <div className={styles.directionsHeading}>
                            <h5>Position Direction</h5>
                        </div>
                        <div className={styles.directions}>
                            <div
                                className={styles.radioWrapper}
                                onClick={(e) => onBullishDirectionButtonClick(e)}
                            >
                                <input
                                    type="radio"
                                    onChange={(e) => onBullishDirectionButtonClick(e)}
                                    checked={futures.position.directionBullish}
                                    className={styles.directionBullish}
                                    name="directionSelected"
                                />
                                <span
                                    className={styles.radioLabel}
                                    onClick={(e) => onBullishDirectionButtonClick(e)}
                                >
                                    Bullish
                                </span>
                            </div>
                            <div
                                className={styles.radioWrapper}
                                onClick={(e) => onBearishDirectionButtonClick(e)}
                            >
                                <input
                                    type="radio"
                                    onChange={(e) => onBearishDirectionButtonClick(e)}
                                    checked={!futures.position.directionBullish}
                                    className={styles.directionBearish}
                                    name="directionSelected"
                                />
                                <span
                                    className={styles.radioLabel}
                                    onClick={(e) => onBearishDirectionButtonClick(e)}
                                >
                                    Bearish
                                </span>
                            </div>
                        </div>

                        <div className={styles.entryPriceHeading}>
                            <h5>Entry Price</h5>
                        </div>
                        <div className={styles.entryPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => entryPriceOnChange(e)}
                                value={futures.position.entryPrice}
                            />
                        </div>

                        <div className={styles.exitPriceHeading}>
                            <h5>Exit Price</h5>
                        </div>
                        <div className={styles.exitPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => exitPriceOnChange(e)}
                                value={futures.position.exitPrice}
                            />
                        </div>

                        <div className={styles.contractsNumberHeading}>
                            <h5>Number of Contract</h5>
                        </div>
                        <div className={styles.contractsNumberChange}>
                            <input
                                min={0}
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => contractsNumberOnChange(e)}
                                value={futures.position.numberOfContracts}
                            />
                        </div>
                    </div>
                    <aside className={styles.contractInformationContainer}>
                        <div className={styles.contractNameContainer}>
                            <h5>{futures.chosenFutures.name}</h5>
                        </div>
                        <div className={styles.tickSizeHeadingContainer}>
                            <h5>Tick Size</h5>
                        </div>
                        <div className={styles.tickSizeValueContainer}>
                            <h5>{futures.chosenFutures.tickSize}</h5>
                        </div>
                        <div className={styles.tickValueHeadingContainer}>
                            <h5>Tick Value</h5>
                        </div>
                        <div className={styles.tickValueContainer}>
                            <h5>${futures.chosenFutures.tickValue}</h5>
                        </div>
                    </aside>
                </div>
            </form>
            <div className={styles.calculateButtonContainer}>
                <div className={styles.calculateButton} onClick={(e) => CalculatePLResult(e)}>
                    Calculate
                </div>
            </div>

            <div className={styles.answerWrapper}>
                <h3 className={styles.resultsLabel}>Profit or Loss Results</h3>
                <div className={styles.answerContainer}>
                    <div className={styles.ticksHeading}>
                        <h5>Ticks</h5>
                    </div>
                    <div className={styles.ticksValue}>
                        {futures.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                {Math.abs(futures.answer.ticks).toFixed(2)}
                            </h5>
                        ) : futures.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>
                                {Math.abs(futures.answer.ticks).toFixed(2)}
                            </h5>
                        ) : (
                            <h5>{Math.abs(futures.answer.ticks).toFixed(2)}</h5>
                        )}
                    </div>

                    <div className={styles.dollarsHeading}>
                        <h5>USD($)</h5>
                    </div>
                    <div className={styles.dollarsValue}>
                        {futures.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                -${Math.abs(futures.answer.USDValue).toFixed(2)}
                            </h5>
                        ) : futures.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>
                                ${futures.answer.USDValue.toFixed(2)}
                            </h5>
                        ) : (
                            <h5>${futures.answer.USDValue.toFixed(2)}</h5>
                        )}
                    </div>

                    <div className={styles.saveToPositionsContainer}>
                            
                    </div>
                </div>
            </div>
        </div>
    );
};
