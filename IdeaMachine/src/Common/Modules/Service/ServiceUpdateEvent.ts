// Framework
import AsyncEvent from "common/helper/AsyncEvent";

// Types
import { ServiceNotification } from "./types";

export const ServiceUpdateEvent = new AsyncEvent<ServiceNotification>();
export default ServiceUpdateEvent;