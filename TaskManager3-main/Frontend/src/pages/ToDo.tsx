import AddTaskForm from "../components/ApiFetch";
import React, { useState } from "react";
import { GetAllTasks } from "../components/ApiGet";
import { deleteTask } from "../components/Delete";
import { GetTaskId } from "../components/Edit";
import { EditTaskForm } from "../components/Edit";
import { MarkTaskDone } from "../components/Status";
import Task from "../models/ToDo";
import {
  Accordion,
  Button,
  Container,
  Dropdown,
  Col,
  Row,
} from "react-bootstrap";
import "../App.css";

const Tasks = () => {
  const [showForm, setShowForm] = useState(false);
  const [showEditForm, setShowEditForm] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState<string | null>(null);
  const [selectedActivity, setSelectedActivity] = useState<string | null>(
    null
  );
  const [selectedTaskId, setSelectedTaskId] = useState<number | null>(null);

  const handleButtonClick = () => {
    setShowForm(true);
  };

  const handleEditButtonClick = (taskId: number) => {
    setShowEditForm(true);
    setSelectedTaskId(taskId);
    GetTaskId(taskId);
    console.log(GetTaskId(taskId));
  };


  const handleStatusFilter = (status: string) => {
    if (selectedStatus === status) {
      setSelectedStatus(null);
    } else {
      setSelectedStatus(status);
    }
  };

  const handleActivityFilter = (activityfilter: string) => {
    if (selectedActivity === activityfilter) {
      setSelectedActivity(null);
    } else {
      setSelectedActivity(activityfilter);
    }
  };



  const isStatusVisible = (task: Task) => {
    return !selectedStatus || task.StatusName === selectedStatus;
  };

  const isActivityTypeVisible = (task: Task) => {
    return !selectedActivity || task.ActivityTypeName === selectedActivity;
  };

  const isTaskVisible = (task: Task) => {
    return (
      isStatusVisible(task) &&
      isActivityTypeVisible(task)
    );
  };

  return (
    <Container className="text-center mt-4" style={{ maxWidth: "800px" }}>
      {showForm ? (
        <Container className="d-flex flex-column align-items-center">
          <h2 className="mb-4">Uusi tehtävä</h2>
          <AddTaskForm />
        </Container>
      ) : (
        <Container className="d-flex justify-content-between align-items-center container bg-custom-primary p-4">
          <h2 className="mb-0 mr-auto text-custom-white">Tehtävät</h2>
          <Button className="btn btn-custom-primary" onClick={handleButtonClick}>
            Uusi tehtävä
          </Button>
        </Container>
      )}
      {!showForm && (
        <GetAllTasks>
          {({ tasks }) => (
            <div>
              {tasks.length === 0 ? (
                <h3>Ei tehtäviä</h3>
              ) : (
                <>
                  <Container className="d-flex justify-content-between align-items-center  bg-custom-light p-4">
                    <h3>Suodatin</h3>
                    <div className="d-flex">
                      <Row>
                        <Col md="4">
                        </Col>
                        <Col md="4">
                          <Dropdown>
                            <Dropdown.Toggle
                              variant="custom-secondary"
                              id="dropdown-status"
                            >
                              {selectedStatus ? selectedStatus : "Tila"}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                              <Dropdown.Item
                                onClick={() => handleStatusFilter("")}
                              >
                                Tyhjä
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() => handleStatusFilter("Uusi")}
                              >
                                Uusi
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() =>
                                  handleStatusFilter("Keskeneräinen")
                                }
                              >
                                Keskeneräinen
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() =>
                                  handleStatusFilter("Vanhentunut")
                                }
                              >
                                Vanhentunut
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() => handleStatusFilter("Valmis")}
                              >
                                Valmis
                              </Dropdown.Item>
                            </Dropdown.Menu>
                          </Dropdown>
                        </Col>
                        <Col md="4">
                          <Dropdown>
                            <Dropdown.Toggle
                              variant="custom-secondary"
                              id="dropdown-activity"
                            >
                              {selectedActivity ? selectedActivity : "Tyyppi"}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                              <Dropdown.Item
                                onClick={() => handleActivityFilter("")}
                              >
                                Tyhjä
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() => handleActivityFilter("Koulu")}
                              >
                                Koulu
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() => handleActivityFilter("Työ")}
                              >
                                Työ
                              </Dropdown.Item>
                              <Dropdown.Item
                                onClick={() =>
                                  handleActivityFilter("Harrastus")
                                }
                              >
                                Harrastus
                              </Dropdown.Item>
                            </Dropdown.Menu>
                          </Dropdown>
                        </Col>
                      </Row>
                    </div>
                  </Container>
                  <Accordion>
                    {tasks
                      .filter(isTaskVisible)
                      .map((task: Task, TaskIndex: number) => (
                        <Accordion.Item
                          key={TaskIndex}
                          eventKey={`${TaskIndex}`}
                        >
                          <Accordion.Header>{task.Name}</Accordion.Header>
                          <Accordion.Body className="accordion-body" style={{ textAlign: "left" }}>
                            <h4>Kuvaus</h4>
                            <p>{task.Description}</p>
                            <h4>Aloitusaika</h4>
                            <p>{task.StartDate.toLocaleString()}</p>
                            <h4>Lopetusaika</h4>
                            <p>{task.EndDate.toLocaleString()}</p>
                            <h4>Tila</h4>
                            <p>{task.StatusName}</p>
                            <h4>Aktiviteetin tyyppi</h4>
                            <p>{task.ActivityTypeName}</p>
                            <Button
                              variant="custom-success"
                              onClick={() => MarkTaskDone(task.Id)}
                              style={{ marginRight: "10px" }}
                            >
                              Valmis
                            </Button>
                            <Button
                              variant="custom-danger"
                              onClick={() => deleteTask(task.Id)}
                              style={{ marginRight: "10px" }}
                            >
                              Poista
                            </Button>
                            <Button onClick={() => handleEditButtonClick(task.Id)}>
                              Muokkaa
                            </Button>
                            {showEditForm && selectedTaskId === task.Id && (
                              <EditTaskForm taskId={selectedTaskId} />
                            )}
                          </Accordion.Body>
                        </Accordion.Item>
                      ))}
                  </Accordion>
                </>
              )}
            </div>
          )}
        </GetAllTasks>
      )}
    </Container>
  );
};

export default Tasks;
