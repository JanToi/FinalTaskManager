import { GetAllTasks } from "../components/ApiGet";
import { Accordion, Card, Row, Col, Container } from "react-bootstrap";
import Task from "../models/ToDo";
import Activity from "../models/Activity";
import "../App.css";

const Home = () => {
  const TaskSection: React.FC<{ tasks: Task[] }> = ({ tasks }) => (
    <>
      <Col md="6">
        
          <h1>Viimeaikaiset tehtävät</h1>
        
        <Accordion>
          {tasks.slice(0, 5).map((task, index) => (
            <Card key={index}>
              <Accordion.Item eventKey={index.toString()}>
                <Accordion.Header>{task.Name}</Accordion.Header>
                <Accordion.Body
                  className="accordion-body"
                  style={{ textAlign: "left" }}
                >
                  <h4>Kuvaus</h4>
                  <p>{task.Description}</p>
                  <h4>Aloitusaika</h4>
                  <p>{task.StartDate.toLocaleString()}</p>
                  <h4>Lopetusaika</h4>
                  <p>{task.EndDate.toLocaleString()}</p>
                  <h4>Tila</h4>
                  <p>{task.StatusName}</p>
                  <h4>Tyyppi</h4>
                  <p>{task.ActivityTypeName}</p>
                </Accordion.Body>
              </Accordion.Item>
            </Card>
          ))}
        </Accordion>
      </Col>
    </>
  );

  const ActivitySection: React.FC<{ activities: Activity[] }> = ({
    activities,
  }) => {
    return (
      <>
        <Col md="6">
          
            <h1>Viimeaikaiset aktiviteetit</h1>
          
          <Accordion>
            {activities.slice(0, 5).map((activity, index) => (
              <Accordion key={index}>
                <Accordion.Item eventKey={index.toString()}>
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
                    <h4>Tyyppi</h4>
                    <p>{activity.ActivityTypeName}</p>
                  </Accordion.Body>
                </Accordion.Item>
              </Accordion>
            ))}
          </Accordion>
        </Col>
      </>
    );
  };

  return (
    <Container className="text-center mt-4" style={{ maxWidth: "800px" }}>
      <GetAllTasks>
        {({ tasks, activities }) => (
          <>
            {tasks.length === 0 && activities.length === 0 ? (
              
                <h1>Ei tehtäviä tai aktiviteettejä</h1>
              
            ) : (
              <Row>
                {tasks.length > 0 && <TaskSection tasks={tasks} />}
                {tasks.length === 0 && activities.length > 0 && (
                  <Col md="6">
                    
                      <h1>Ei tehtäviä</h1>
                    
                  </Col>
                )}
                {activities.length > 0 && (
                  <ActivitySection activities={activities} />
                )}
                {activities.length === 0 && tasks.length > 0 && (
                  <Col md="6">
                    
                      <h1>Ei aktiviteettejä</h1>
                    
                  </Col>
                )}
              </Row>
            )}
          </>
        )}
      </GetAllTasks>
    </Container>
  );
};

export default Home;
