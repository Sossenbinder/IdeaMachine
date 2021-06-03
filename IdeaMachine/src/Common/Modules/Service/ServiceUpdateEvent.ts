// Framework
import AsyncEvent from "common/Helper/AsyncEvent";

// Types
import { ServiceNotification } from "./types";

export const ServiceUpdateEvent = new AsyncEvent<ServiceNotification>();
export default ServiceUpdateEvent;