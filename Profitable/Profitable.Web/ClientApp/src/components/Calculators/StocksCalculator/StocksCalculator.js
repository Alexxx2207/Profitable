import { useReducer } from "react";
import classnames from "classnames";

import { calculateStockTrade } from "../../../services/stocks/stocksService";

import styles from "./StocksCalculator.module.css";

const initialState = {
    position: {
        entryPrice: 0,
        exitPrice: 0,
        entryCommission: 0,
        exitCommission: 0,
        numberOfShares: 0,
    },
    answer: {
        USDValue: 0,
    },
};

function stocksReducer(state, action) {
    switch (action.type) {
        case "loadAllStocks":
            return {
                ...state,
                allStocks: action.allStocks,
                chosenStocks: action.allStocks[0],
            };
        case "selectShareEntryPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryPrice: action.entryPrice,
                },
            };
        case "selectShareExitPrice":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitPrice: action.exitPrice,
                },
            };
        case "selectShareEntryCommission":
            return {
                ...state,
                position: {
                    ...state.position,
                    entryCommission: action.entryCommission,
                },
            };
        case "selectShareExitCommission":
            return {
                ...state,
                position: {
                    ...state.position,
                    exitCommission: action.exitCommission,
                },
            };
        case "selectSharesCount":
            if (action.numberOfShares >= 0) {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfShares: action.numberOfShares,
                    },
                };
            } else {
                return {
                    ...state,
                    position: {
                        ...state.position,
                        numberOfShares: 0,
                    },
                };
            }
        case "setResult":
            return {
                ...state,
                answer: {
                    USDValue: action.USDValue,
                },
            };
        default:
            break;
    }
}

export const StocksCalculator = () => {
    const [stocks, dispatch] = useReducer(stocksReducer, initialState);

    const entryPriceOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectShareEntryPrice", entryPrice: e.target.value });
    };

    const exitPriceOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectShareExitPrice", exitPrice: e.target.value });
    };

    const entryCommissionOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectShareEntryCommission", entryCommission: e.target.value });
    };

    const exitCommissionOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectShareExitCommission", exitCommission: e.target.value });
    };

    const sharesNumberOnChange = (e) => {
        e.preventDefault();
        dispatch({ type: "selectSharesCount", numberOfShares: e.target.value });
    };

    const CalculatePLResult = (e) => {
        e.preventDefault();

        const plResult = calculateStockTrade(
            Number(stocks.position.numberOfShares),
            Number(stocks.position.exitPrice),
            Number(stocks.position.entryPrice),
            Number(stocks.position.exitCommission),
            Number(stocks.position.entryCommission)
        );

        dispatch({ type: "setResult", USDValue: plResult });
    };

    return (
        <div className={styles.stocksCalculatorWrapper}>
            <h2 className={styles.stocksCalculatorHeading}>Stocks Calculator</h2>
            <form className={styles.stocksCalculatorForm}>
                <div className={styles.stocksInformation}>
                    <div className={styles.positionInformationContainer}>
                        <div className={styles.entryPriceHeading}>
                            <h5>Entry Price</h5>
                        </div>
                        <div className={styles.entryPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => entryPriceOnChange(e)}
                                value={stocks.position.entryPrice}
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
                                value={stocks.position.exitPrice}
                            />
                        </div>

                        <div className={styles.sharesNumberHeading}>
                            <h5>Number of Share</h5>
                        </div>
                        <div className={styles.sharesNumberChange}>
                            <input
                                min={0}
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => sharesNumberOnChange(e)}
                                value={stocks.position.numberOfShares}
                            />
                        </div>
                    </div>
                    <div
                        className={classnames(
                            styles.positionInformationContainer,
                            styles.positionInformationCommisionContainer
                        )}
                    >
                        <div className={styles.entryPriceHeading}>
                            <h5>Buy Commission</h5>
                            <h6>In $</h6>
                        </div>
                        <div className={styles.entryPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => entryCommissionOnChange(e)}
                                value={stocks.position.entryCommission}
                            />
                        </div>

                        <div className={styles.exitPriceHeading}>
                            <h5>Sell Commission</h5>
                            <h6>In $</h6>
                        </div>
                        <div className={styles.exitPriceChange}>
                            <input
                                className={styles.numberInput}
                                type="number"
                                onChange={(e) => exitCommissionOnChange(e)}
                                value={stocks.position.exitCommission}
                            />
                        </div>
                    </div>
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
                    <div className={styles.dollarsHeading}>
                        <h5>USD($)</h5>
                    </div>
                    <div className={styles.dollarsValue}>
                        {stocks.answer.USDValue < 0 ? (
                            <h5 className={styles.redPL}>
                                -${Math.abs(stocks.answer.USDValue).toFixed(2)}
                            </h5>
                        ) : stocks.answer.USDValue > 0 ? (
                            <h5 className={styles.greenPL}>${stocks.answer.USDValue.toFixed(2)}</h5>
                        ) : (
                            <h5>${stocks.answer.USDValue.toFixed(2)}</h5>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};
