import React, { Component } from 'react';
import { Home } from './components/Home/Home';
import './custom.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Home />
    );
  }
}
