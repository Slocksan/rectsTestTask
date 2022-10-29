import React, { useState, useEffect, useRef } from 'react';
import { Button, Input, Label, Row, Col } from 'reactstrap';
import axios from 'axios';
import randomcolor from 'randomcolor';

export function RectsTask(props) {
  const canvas = useRef();
  const [frequency, setFrequency] = useState(1000);
  const [iterationToDeath, setIterationToDeath] = useState(2);
  const [isTicking, setTicking] = useState(false);
  const [count, setCount] = useState(0);
  const [ctx, setCtx] = useState();
  const [rectangles, setRectangles] = useState([]);

  const handleFrequencyChange = (e) => setFrequency(e.target.value);
  const handleIterationToDeathChange = (e) => setIterationToDeath(e.target.value);

  useEffect(() => {
    const canvasEle = canvas.current;
    canvasEle.width = canvasEle.clientWidth;
    canvasEle.height = canvasEle.clientHeight;
 
    setCtx(canvasEle.getContext("2d"));
  }, []);

  useEffect(() => {
    console.log("ctx changed");
    const newRectanglesArray = [];

    ctx && axios.get("/Rectangles/get_all_rectangles").then(r => {
      r.data.forEach(element => {
        newRectanglesArray.push({x: element.position.x, y: element.position.y, w: element.size.x, h: element.size.y, backgroundColor: randomcolor()});
      });

      setRectangles(newRectanglesArray);
      rerfreshCanvas();
    }); 
  }, [ctx]);

   useEffect(() => {
    const timer = setTimeout(() => 
    { if (isTicking) {
        setCount(count+1);
        iterate();
      } 
    }, frequency);

    return () => clearTimeout(timer)
   }, [count, frequency, isTicking]);

   const rerfreshCanvas = () => {
    console.log(canvas.current);
    canvas.current.getContext("2d").clearRect(0, 0, canvas.current.width, canvas.current.height);

    rectangles.forEach(element => {
      drawRect({x:element.x, y:element.y, w:element.w, h:element.h}, {backgroundColor: element.backgroundColor});
    });
  }

   const drawRect = (info, style = {}) => {
    const { x, y, w, h } = info;
    const { backgroundColor = 'black' } = style;
 
    ctx.beginPath();
    ctx.fillStyle = backgroundColor;
    ctx.fillRect(x, y, w, h);
  }

  const reset = () => {
    setCount(0);
    setTicking(false);

    axios.post(`/Rectangles/reset?xSize=${canvas.current.clientWidth}&ySize=${canvas.current.clientHeight}&iterationsToDie=${iterationToDeath}`);
    setRectangles([]);
    rerfreshCanvas();
  }

  const iterate = () => {
    ctx && axios.get("/Rectangles/iterate").then(r => {
      const newRect = r.data.added;
      const rectsToDelete = r.data.removed;

      rectsToDelete.forEach(element => {
        const rectIndex = rectangles.findIndex(item => element.position.x == item.x && element.position.y == item.y && element.size.x == item.w && element.size.y == item.h);
      
        if (rectIndex != -1) {
          rectangles.splice(rectIndex,1);
        }
      });

      rectangles.push({x: newRect.position.x, y: newRect.position.y, w: newRect.size.x, h: newRect.size.y, backgroundColor: randomcolor()});
      setRectangles(rectangles);

      rerfreshCanvas();
    });
  }

    return (
      <div>
          <Row xs="2">
            <Col>
              <Label>Update frequency (mc)</Label>
            </Col>
            <Col>
              <Label><Input value={frequency} onChange={handleFrequencyChange}></Input></Label>
            </Col>
            <Col>
              <Label><Label>Iteration to death</Label></Label>
            </Col>
            <Col>
              <Label><Input value={iterationToDeath} onChange={handleIterationToDeathChange}></Input></Label>
            </Col>       
          </Row>
          <Row xs="3">
            <Col>
              <Label>{count}</Label>
            </Col> 
            <Col>
              <Button block onClick={() => setTicking(!isTicking)}>start/stop</Button>
            </Col> 
            <Col>
              <Button block onClick={reset}>apply and clear</Button>
            </Col> 
          </Row>  
        <canvas ref={canvas}></canvas>
      </div>
    );
}