import AddActivityForm from "../components/ApiPush";
import React, { useState } from "react";
import {
  Container,
  Accordion,
  Button,
  Dropdown,
  Col,
  Row,
} from "react-bootstrap";
import { GetAllTasks } from "../components/ApiGet";
import { deleteActivity } from "../components/Delete";
import { GetTaskId } from "../components/Edit";
import { EditActivityForm } from "../components/Edit";
import { MarkActivityDone } from "../components/Status";
import Activity from "../models/Activity";
import "../App.css";

const Activities = () => {
  const [showForm, setShowForm] = useState(false);
  const [showEditForm, setShowEditForm] = useState(false);
  const [selectedStatus, setSelectedStatus] = useState<string | null>(null);
  const [selectedActivity, setSelectedActivity] = useState<string | null>(
    null
  );
  const [selectedActivityId, setSelectedActivityId] = useState<number | null>(
    null
  );

  const handleButtonClick = () => {
    setShowForm(true);
  };

  const handleEditButtonClick = (activityId: number) => {
    setShowEditForm(true);
    setSelectedActivityId(activityId);
    GetTaskId(activityId);
    console.log(GetTaskId(activityId));
  };


  const handleStatusFilter = (status: string) => {
    setSelectedStatus(status === selectedStatus ? null : status);
  };

  const handleActivityFilter = (activityType: string) => {
    setSelectedActivity(activityType === selectedActivity ? null : activityType);
  };


  const isStatusVisible = (activity: Activity) => {
    return !selectedStatus || activity.StatusName === selectedStatus;
  };

  const isActivityTypeVisible = (activity: Activity) => {
    return !selectedActivity || activity.ActivityTypeName === selectedActivity;
  };

  const isActivityVisible = (activity: Activity) => {
    return (
      isStatusVisible(activity) &&
      isActivityTypeVisible(activity)
    );
  };

  return (
    <Container className="text-center mt-4" style={{ maxWidth: "800px" }}>
      {showForm ? (
        <Container className="d-flex flex-column align-items-center">
          <h2 className="mb-4">Uusi aktiviteetti</h2>
          <AddActivityForm />
        </Container>
      ) : (
        <Container className="d-flex justify-content-between align-items-center container bg-custom-primary p-4">
          <h2 className="mb-0 mr-auto text-custom-white">Aktiviteetit</h2>
          <Button className="btn btn-custom-primary" onClick={handleButtonClick}>
            Uusi aktiviteetti
          </Button>
        </Container>
      )}
      {!showForm && (
        <GetAllTasks>
          {({ activities }) => (
            <div>
              {activities.length === 0 ? (
                <h3>Ei aktiviteettejä</h3>
              ) : (
                <>
                  <Container className="d-flex justify-content-between align-items-center  bg-custom-light p-4">
                    <h3>Suodattimet</h3>
                    <div className="d-flex">
                      <Row>
                        <Col md="4">
                        </Col>
                        <Col md="4">
                          <Dropdown>
                            <Dropdown.Toggle variant="custom-secondary" id="dropdown-status">
                              {selectedStatus ? selectedStatus : "Tila"}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                              <Dropdown.Item onClick={() => handleStatusFilter("")}>
                                Tyhjä
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleStatusFilter("New")}>
                                Uusi
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleStatusFilter("In Progress")}>
                                Keskeneräinen
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleStatusFilter("Expired")}>
                                Vanhentunut
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleStatusFilter("Done")}>
                                Valmis
                              </Dropdown.Item>
                            </Dropdown.Menu>
                          </Dropdown>
                        </Col>
                        <Col md="4">
                          <Dropdown>
                            <Dropdown.Toggle variant="custom-secondary" id="dropdown-activity">
                              {selectedActivity ? selectedActivity : "Tyyppi"}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                              <Dropdown.Item onClick={() => handleActivityFilter("")}>
                                Tyhjä
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleActivityFilter("School")}>
                                Koulu
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleActivityFilter("Work")}>
                                Työ
                              </Dropdown.Item>
                              <Dropdown.Item onClick={() => handleActivityFilter("Hobby")}>
                                Harrastus
                              </Dropdown.Item>
                            </Dropdown.Menu>
                          </Dropdown>
                        </Col>
                      </Row>
                    </div>
                  </Container>
                  <Accordion>
                    {activities
                      .filter(isActivityVisible)
                      .map((activity: Activity, activityIndex: number) => (
                        <Accordion.Item
                          key={activityIndex}
                          eventKey={`${activityIndex}`}
                        >
                          <Accordion.Header>{activity.Name}</Accordion.Header>
                          <Accordion.Body
                            className="accordion-body"
                            style={{ textAlign: "left" }}
                          >
                            <h4>Kuvaus</h4>
                            <p>{activity.Description}</p>
                            <h4>Lisätietoja</h4>
                            <p>{activity.Url}</p>
                            <h4>Aloitusaika</h4>
                            <p>{activity.StartDate.toLocaleString()}</p>
                            <h4>Lopetusaika</h4>
                            <p>{activity.EndDate.toLocaleString()}</p>
                            <h4>Tila</h4>
                            <p>{activity.StatusName}</p>
                            <h4>Aktiviteetin tyyppi</h4>
                            <p>{activity.ActivityTypeName}</p>
                            <Button
                              variant="custom-success"
                              onClick={() => MarkActivityDone(activity.Id)}
                              style={{ marginRight: "10px" }}
                            >
                              Valmis
                            </Button>
                            <Button
                              variant="custom-danger"
                              onClick={() => deleteActivity(activity.Id)}
                              style={{ marginRight: "10px" }}
                            >
                              Poista
                            </Button>
                            <Button onClick={() => handleEditButtonClick(activity.Id)}>
                              Muokkaa
                            </Button>
                            {showEditForm && selectedActivityId === activity.Id && (
                              <EditActivityForm taskId={selectedActivityId} />
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

export default Activities;
