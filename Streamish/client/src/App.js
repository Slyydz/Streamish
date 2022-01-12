import React from "react";
import "./App.css";
import VideoList from "./components/VideoList";
import { InputCards } from "./components/Search";

function App() {
  return (
    <div className="App">
      <InputCards />
      <br></br>
      <VideoList />
    </div>
  );
}

export default App;