﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcOpen.Inxton.Swift;
using Vortex.Connector;
using TcoCore;
using System.Text.RegularExpressions;

namespace TcoDrivesBeckhoff
{
    public partial class TcoDriveSimple
    {
        private const string blank = " ";
        private string onlineMsg = string.Empty; string additionalInfo = " ";
        partial void PexConstructor(IVortexObject parent, string readableTail, string symbolTail)
        {
            this._stopTask.LogPayloadDecoration = () => this.GetPlainFromOnline();
            this._haltTask.LogPayloadDecoration = () => this.GetPlainFromOnline();
            this._setPositionTask.LogPayloadDecoration = () => this._setPositionTask.GetPlainFromOnline();
            this._soEResetTask.LogPayloadDecoration = () => this.GetPlainFromOnline();
            this._resetTask.LogPayloadDecoration = () => this.GetPlainFromOnline();
            this._homeTask.LogPayloadDecoration = () => this._homeTask.GetPlainFromOnline();
            this._moveAbsoluteTask.LogPayloadDecoration = () => this._moveAbsoluteTask.GetPlainFromOnline();
            this._moveRelativeTask.LogPayloadDecoration = () => this._moveRelativeTask.GetPlainFromOnline();
            this._moveModuloTask.LogPayloadDecoration = () => this._moveModuloTask.GetPlainFromOnline();
            this._moveVelocityTask.LogPayloadDecoration = () => this._moveVelocityTask.GetPlainFromOnline();

            this._moveAbsoluteTask.CodeProvider = new MoveAbsoluteTaskCodeProvider(this);
            this._moveRelativeTask.CodeProvider = new MoveRelativeTaskCodeProvider(this);
            this._moveModuloTask.CodeProvider = new MoveModuloTaskCodeProvider(this);
            this._moveVelocityTask.CodeProvider = new MoveVelocityTaskCodeProvider(this);
        }


        public string AdvancedDiagnosticMessage
        {

            get
            {

                if (onlineMsg == _messenger._mime.Text.Cyclic)
                {
                    return additionalInfo;
                }


                onlineMsg = _messenger._mime.Text.Synchron;
                additionalInfo = blank;
                var numberFromString = string.Join(string.Empty, Regex.Matches(onlineMsg, @"\d+").OfType<Match>().Select(m => m.Value));


                uint errorCode;

                UInt32.TryParse(numberFromString, out errorCode);
                if (NcErrors.Errors.ContainsKey(errorCode))
                    additionalInfo = NcErrors.Errors.Where(key => key.Key == errorCode).FirstOrDefault().Value;
            

                return additionalInfo;

            }
        }
    }



    public static class NcErrors
    {
        public static IDictionary<uint, string> Errors = new Dictionary<uint, string>()
        {
            {0, "" },
            {16384, @"'Internal error' Internal system error in the NC on ring 0, no further details."},
            {16385, @"'Memory error' The ring-0 memory management is not providing the required memory. This is usually a result of another error, as a result of which the controller will halt normal operation (now if not before)."},
            {16386, @"'Nc retain data error (persistent data)' Error while loading the Nc retain data. The axes concerned are no longer referenced (status flag 'Homed' is set to FALSE).
                    Possible reasons are:
                    - Nc retain data not found
                    - Nc retain data expired (old backup data)
                    - Nc retain data corrupt or inconsistent"},
            {16387, @"'Parameter for Monitoring the NC Setpoint Issuing is Invalid
                    The parameter for activating or deactivating the function 'cyclic monitoring of NC setpoint issuing on continuity and consistency' is invalid. (Special function.)"},
            {16388, @"'External Error
                    This error code can be set by an external module (e.g. third-party module) or can be set when an external module exhibits an error."},
            {16400, @"'Channel identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a channel that does not exist in the system has been named."},
            {16401, @"'Group identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a group that does not exist in the system has been named."},
            {16402, @"'Axis identifier not allowed' Either an unacceptable value (not 1...255) has been used, or an axis that does not exist in the system has been named."},
            {16403, @"'Encoder identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a encoder that does not exist in the system has been named."},
            {16404, @"'Controller identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a controller that does not exist in the system has been named."},
            {16405, @"'Drive identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a drive that does not exist in the system has been named."},
            {16406, @"'Table identifier not allowed' Either an unacceptable value (not 1...255) has been used, or a table that does not exist in the system has been named."},
            {16416, @"'No process image' No PLC-axis interface during creation of an axis."},
            {16417, @"'No process image' No axis-PLC interface during creation of an axis."},
            {16418, @"'No process image' No encoder-I/O interface during creation of an axis."},
            {16419, @"'No process image' No I/O-encoder interface during creation of an axis."},
            {16420, @"'No process image' No drive-I/O interface during creation of an axis."},
            {16421, @"'No process image' No I/O-drive interface during creation of an axis."},
            {16432, @"'Coupling type not allowed' Unacceptable master/slave coupling type."},
            {16433, @"'Axis type not allowed' Unacceptable type specification during creation of an axis."},
            {16434, @"'Unknown Channel Type
                    The NC channel type is unknown. Known types are e.g. an NCI channel, a FIFO channel, etc.."},
            {16448, @"'Axis is incompatible' Axis is not suitable for the intended purpose. A high speed/low speed axis, for example, cannot function as a slave in an axis coupling."},
            {16464, @"'Channel not ready for operation' The channel is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
            {16465, @"'Group not ready for operation' The group is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
            {16466, @"'Axis not ready for operation' The axis is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
            {16480, @"'Channel exists' The channel that is to be created already exists."},
            {16481, @"'Group exists' The group that is to be created already exists."},
            {16482, @"'Axis exists' The axis that is to be created already exists."},
            {16483, @"'Table exists' The table that is to be created already exists, resp. it is tried internally to use an already existing table id ( e.g. for the universal flying saw)."},
            {16496, @"'Axis index not allowed' The location within the channel specified for an axis is not allowed."},
            {16497, @"'Axis index not allowed' The location within the group specified for an axis is not allowed."},
            {16641, @"'Group index not allowed' The location within the channel specified for a group is not allowed."},
            {16642, @"'Null pointer' The pointer to the group is invalid. This is usually a consequence of an error at system start-up."},
            {16643, @"'No process image' It is not possible to exchange data with the PLC. Possible causes: n the channel does not have an interface (no interpreter present) n The connection to the PLC is faulty"},
            {16644, @"'M-function index not allowed' Unacceptable M-function (not 0...159) detected at the execution level."},
            {16645, @"'No memory' No more system memory is available. This is usually the result of another error."},
            {16646, @"'Not ready' The function is not presently available, because a similar function is already being processed. This is usually the result of access conflicts: more than one instance wants to issue commands to the channel. This can, for example, be the consequence of an incorrect PLC //program."},
            {16647, @"'Function/command not supported' A requested function or command is not supported by the channel."},
            {16648, @"'Invalid parameter while starting' Parameters to start the channel (TwinCAT-Start) are invalid. Typically there is an invalid memory size or channel type requested."},
            {16649, @"'Channel function/command not executable' A channel function e.g. interpreter start is not executable because the channel is already busy, no program is loaded or in an error state."},
            {16650, @"'ItpGoAhead not executable' The requested command is not executable, because the interpreter is not executing a decoder stop."},
            {16656, @"'Error opening a file' The specified file does not exist. Sample: NC program unknown."},
            {16657, @"'Syntax error during loading' The NC has found a syntax error when loading an NC program."},
            {16658, @"'Syntax error during interpretation' The NC has found a syntax error when executing an NC program."},
            {16659, @"'Missing subroutine' The NC has found a missing subroutine while loading."},
            {16660, @"'Loading buffer of interpreter is too small' The capacity of the interpreter loading buffer has been exceeded."},
            {16661, @"'Symbolic' - reserved"},
            {16662, @"'Symbolic' - reserved"},
            {16663, @"'Subroutine incomplete' Header of subroutine is missing"},
            {16664, @"'Error while loading the NC program' The maximum number of loadable NC programs has been reached.
                    Possible cause:
                    Too many sub-programs were loaded from a main program."},
            {16665, @"'Error while loading the NC program' The program name is too long."},
            {16672, @"'Divide by zero' The NC encountered a computation error during execution: division by 0."},
            {16673, @"'Invalid circle parameterization' The NC encountered a computation error during execution: The specified circle cannot be calculated."},
            {16674, @"'Invalid FPU-Operation' The NC encountered an invalid FPU-Operation during execution. This error occurs e.g. by calculating the square root of a negative number."},
            {16688, @"'Stack overflow: subroutines' The NC encountered a stack overflow during execution: too many subroutine levels."},
            {16689, @"'Stack underflow: subroutines' The NC encountered a stack underflow during execution: too many subroutine return commands. Note: A main program must not end with a return command."},
            {16690, @"'Stack overflow: arithmetic unit' The NC encountered a stack overflow during execution: The calculation is too complex, or has not been correctly written."},
            {16691, @"'Stack underflow: arithmetic unit' The NC encountered a stack underflow during execution: The calculation is too complex, or has not been correctly written."},
            {16704, @"'Register index not allowed' The NC encountered an unacceptable register index during execution: Either the program contains an unacceptable value (not R0...R999) or a pointer register contains an unacceptable value."},
            {16705, @"'Unacceptable G-function index' The NC has encountered an unacceptable G-function (not 0...159) during execution."},
            {16706, @"'Unacceptable M-function index' The NC has encountered an unacceptable M-function (not 0...159) during execution."},
            {16707, @"'Unacceptable extended address' The NC has encountered an unacceptable extended address (not 1...9) during execution."},
            {16708, @"'Unacceptable index to the internal H-function' The NC has encountered an unacceptable internal H-function in the course of processing. This is usually a consequence of an error during loading."},
            {16709, @"'Machine data value unacceptable' While processing instructions the NC has detected an impermissible value for the machine data (MDB) (not 0…7)."},
            {16720, @"'Cannot change tool params here' The NC has encountered an unacceptable change of parameters for the tool compensation during execution. This error occurred for instance by changing the tool radius and programming a circle in the same block."},
            {16721, @"'Cannot calculate tool compensation' The NC has encountered an error by the calculation of the tool compensation."},
            {16722, @"'Tool compensation: The plane for the tool compensation cannot be changed here. This error occurred for instance by changing the tool plane when the compensation is turned on or active."},
            {16723, @"'Tool compensation: The D-Word is missing or invalid by turning on the tool compensation."},
            {16724, @"'Tool compensation: The specified tool radius is invalid because the value is less or equal zero."},
            {16725, @"'Tool compensation: The tool radius cannot be changed here"},
            {16726, @"'Tool compensation: Collision Detection Table is full."},
            {16727, @"'Tool compensation: Internal error while turning on the contour collision detection."},
            {16728, @"'Tool compensation: Internal error within the contour collision detection: update reversed geo failed."},
            {16729, @"'Tool compensation: Unexpected combination of geometry types by active contour collision detection."},
            {16730, @"'Tool compensation: Programmed inner circle is smaller than the cutter radius"},
            {16731, @"'Tool compensation: Bottle neck detection recognized contour violation"},
            {16732, @"'Table for corrected entries is full"},
            {16733, @"'Input table for tangential following is full"},
            {16734, @"'Executing table for tangential following is full"},
            {16735, @"'Geometric entry for tangential following cannot be calculated"},
            {16736, @"'reserved"},
            {16737, @"'reserved"},
            {16738, @"'The actual active interpolation rules (g-code), zero-shifts, or rotation cannot be detected"},
            {16752, @"'Error while loading: Invalid parameter' The NC has found an invalid parameter while loading an NC program."},
            {16753, @"'Invalid contour start position' The NC encountered a computation error during execution: The specified contour cannot be calculated because the initial position is not on the contour."},
            {16754, @"'Retrace: Invalid internal entry index' The NC encountered an invalid internal entry index during execution of the retrace function."},
            {16755, @"'Invalid G Code
                    Invalid default G Code. False expression/syntax in default G Code."},
            {16756, @"'Error while Opening the G Code File
                    Error while opening the default G code file."},
            {16896, @"'Group ID not allowed'	
                    The value for the group ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is greater than 255.	"},
            {16897, @"'Group type not allowed'	
                    The value for the group type is unacceptable because it is not defined.	
                    Type 1: PTP group with slaves (servo)	
                    Type 4: DXD group with slaves (3D group)	
                    Type 5: High/low speed group	
                    Type 6: Stepper motor group	
                    Type 9: Encoder group with slaves (servo)	"},
            {16898, @"'Master axis index not allowed' The value for the master axis index in an interpolating 3D group is not allowed, because, for instance, it has gone outside the value range. Index 0: X axis (first master axis) Index 1: Y axis (second master axis) Index 2 : Z axis (third master //axis)"},
            {16899, @"'Slave axis index not allowed' (INTERNAL ERROR) The value for the slave axis index in a group is not allowed, because, for instance, it has passed outside the value range, the slave location to be used when inserting a new slave connection is already occupied, or because no slave //is present when such a connection is being removed. Index 0: First slave axis Index 1: Second slave axis Index 2: etc."},
            {16900, @"'Internal Error	
                    A nonexpected internal error has occurred. The following situations may have caused this effect:	
                    There is not enough TC router memory or Windows memory to establish the internal NC objects,	
                    internal NC structures and links (pointers between NC objects) are erroneous or are missing,	
                    a fatal internal error in calculating a stop command or a halt command has occurred,	
                    internal checking of NC own logic and algorithms (self-checking software),	
                    nonexpected modes and cases that are not intended regularly, but are recognized being erroneous.	
                    Note: Quite often in such an error situation an additional error message in the Windows event logger is thrown that can be helpful for a more detailed analysis by Beckhoff or by the user.	"},
            {16901, @"'Invalid cycle time for statement execution task (SAF)' The value of the cycle time for the NC block execution task (SAF 1/2) is not allowed, because it has passed outside the value range."},
            {16902, @"'GROUPERR_RANGE_MAXELEMENTSINAXIS '"},
            {16903, @"'Invalid cycle time for the statement preparation task (SVB)' The value of the cycle time for the NC statement preparation task (SVB 1/2) is not allowed, because it has passed outside the value range."},
            {16904, @"'Single step mode not allowed' The flag for the activation or deactivation of single step mode is not allowed. Value 0: Passive (buffered operation) Value 1: Active (single-block operation)"},
            {16905, @"'Group deactivation not allowed' (INTERNAL ERROR) The flag for the deactivation or activation of the complete group is not allowed. Value 0: Group active Value 1: Group passive"},
            {16906, @"'Statement execution state (SAF state) not allowed' (INTERNAL ERROR) The value for the state of the block execution state machine (SAF state) is not allowed. This error occurs on passing outside the range of values, or if the state machine enters an error state."},
            {16907, @"'Channel address' The group does not have a channel, or the channel address has not been initialized."},
            {16908, @"'Axis address (master axis)' The group does not have a master axis (or axes) or the axis address(es) has (have) not been initialized."},
            {16909, @"'Master axis address' A new master/slave coupling is to be inserted into the group, but there is no valid address for the leading master axis."},
            {16910, @"'Slave axis address' A master/slave coupling is to be inserted into the group, but there is no valid address for the slave axis."},
            {16911, @"'Slave set value generator address' A master/slave coupling is to be inserted into the group, but there is no valid address for the slave set value generator."},
            {16912, @"'Encoder address' An axis in the group does not have an encoder, or the encoder address has not been initialized."},
            {16913, @"'Controller address' An axis in the group does not have a controller, or the controller address has not been initialized."},
            {16914, @"'Drive address' An axis in the group does not have a drive, or the drive address has not been initialized."},
            {16915, @"'Address Master Setpoint Generator	
                    A group (e.g. FIFO group) does not own a master setpoint generator or a setpoint generator address has not been initialized. Possibly, there may not be enough memory available.	"},
            {16916, @"'Axis interface NC to PLC address' Group/axis does not have an axis interface from the NC to the PLC, or the axis interface address has not been initialized."},
            {16917, @"'Slave axis address' An existing master/slave coupling is to be removed from the group, but there is no valid address for the slave axis."},
            {16918, @"'Table address unknown' The table, respectively the table ID, is unknown. This table is used for the master/slave coupling or for the characteristic curve."},
            {16919, @"'NcControl address' The NcControl address has not been initialized."},
            {16920, @"'Axis is blocked for commands while persistent NC data are queued' Axis is blocked for commands while waiting for valid IO data to accept the queued persistent NC data."},
            {16921, @"'The scaling mode MASTER-AUTOOFFSET is invalid because no reference table was found'. The used scaling mode MASTER-AUTOOFFSET is invalid in this context because an existing reference table is missing.
                    This error can occur for example when adding cam tables without a unique reference to an existing cam table.    "},
            {16922, @"'The master axis start position does not permit synchronization' When a slave axis is being coupled on, the position of the master axis does not permit synchronization at the given synchronization positions."},
            {16923, @"'Slave coupling factor (gearing factor) of 0.0 is not allowed' A master/slave coupling with a gearing factor of 0.0 is being created. This value is not allowed, since it does not correspond to any possible coupling, and division will generate an FPU exception."},
            {16924, @"'Insertion of master axis into group not allowed' A master axis is to be inserted into a group at a location that is already occupied by another master axis. Maybe the reconfiguration cannot be done, because this axis has got an existing slave coupling. This master/slave coupling must be revoked before."},
            {16925, @"'Deletion of master axis from group not allowed' (INTERNAL ERROR) A master axis is to be removed from a location in a group that is not in fact occupied by master axis."},
            {16926, @"'Function/feature is not supported from the setpoint generator A function or feature is not supported from the setpoint generator (e.g. PTP master setpoint generator). This can be in general or only in a special situation."},
            {16927, @"'Group initialization' Group has not been initialized. Although the group has been created, the rest of the initialization has not been performed (1. Initialization of group I/O, 2. Initialization of group, 3. Reset group)."},
            {16928, @"'Group not ready / group not ready for new task' The group is being given a new task while it is still in the process of executing an existing task. This request is not allowed because it would interrupt the execution of the previous task. The new task could, for instance, be a positioning command, or the 'set actual position' function. Precisely the converse relationships apply for the 'set new end position' function. In that case, the group/axis must still be actively moving in order to be able to cause a change in the end position."},
            {16929, @"'Requested set velocity is not allowed' The value requested for the set velocity of a positioning task is less than or equal to zero, larger than the 'maximum velocity' (see axis parameters), or, in the case of servo-drives, is larger than the 'reference velocity' of the axis (see drive parameters)."},
            {16930, @"'Requested target position is not allowed (master axis)' The requested value for the target position of a positioning task is not within the software end locations. In other words, it is either less than the minimum software end location or larger than the maximum software end location. This check is only carried out if the relevant end position monitoring is active."},
            {16931, @"'No enable for controller and/or feed (Master axis)' The axis enables for the master axis needed for positioning are not present. This can involve the controller enable and/or the relevant, direction-dependent feed enable (see axis interface PlcToNc)."},
            {16932, @"'Movement smaller than one encoder increment' (INTERNAL ERROR) The distance that a group/axis is supposed to move is smaller than the physical significance of one encoder increment. In other words the movement is smaller than the scaling factor of the axis. The reaction to this is that the axis is reported as having logically finished without having actively moved. This means that an external error is not generated for the user. This error is also issued for high/low speed axes if a loop movement with nonzero parameters is smaller than the sum of the creeping and //braking distances. In such a case it is not meaningful to either exceed or to fail to reach the target position."},
            {16933, @"'Drive not ready during axis start' During an axis start it is ascertained that the drive is not ready. The following are possible causes: - the drive is in the error state (hardware error) - the drive is in the start-up phase (e.g. after an axis reset that was preceded by a hardware error) - the drive is missing the controller enable (ENABLE) Note: The time required for 'booting' a drive after a hardware fault can amount to several seconds."},
            {16934, @"'Invalid parameters of the emergency stop.' Either, both, the deceleration and the jerk are less than zero or one of the parameters is weaker than the corresponding parameter of the start data."},
            {16935, @"'The setpoint generator is inactive such that no instructions are accepted.'"},
            {16936, @"'Requested traverse distance is not allowed' The requested traverse distance or looping distance is smaller than the braking distance of the two/speed axis."},
            {16937, @"'Requested target position is not allowed (slave axis)' The value for the target position of a positioning task when calculated for the slave axis is not within the software end locations. In other words, it is either less than the minimum software end location or larger than the maximum software end location. This check is only carried out if the relevant end position monitoring is active."},
            {16938, @"'No enable for controller and/or feed (slave axis)' The axis enables for one or more coupled slave axes needed for positioning are not present. This can involve the controller enable and/or the relevant, direction-dependent feed enable (see axis interface PlcToNc)."},
            {16939, @"'The activation position (position threshold) is out of range of the actual positioning' The activation position (position threshold) of a new axis command (e.g. 'new velocity activated at a position') is out of range. E.g. the activation position is before the actual position or behind the target position."},
            {16940, @"'The start or activation data of the external setpoint generation are not valid' This may be caused through: 1. The external setpoint generation is active and a new activation with a start type (1: absolute, 2: relative) unequal to the current one is send.
                    2. The internal setpoint generation is active(e.g.PTP) and the external one is activated with the type absolute(two setpoint generators of the type absolute are not possible).	"},
            {16941, @"'Velocity is not constant' For changing the dynamic parameter 'acceleration' und 'deceleration' the axis has to be in dynamic state without acceleration and deceleration (that means constant velocity)."},
            {16942, @"'Jerk less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 for the jerk (PTP and CNC) is not allowed, since the jerk is by definition positive, and with a jerk of 0.0, division will generate an FPU exception."},
            {16943, @"'Acceleration less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 for the acceleration (PTP and CNC) is not allowed, since the acceleration is positive by definition, and an acceleration of 0.0 will not allow a motion to be generated."},
            {16944, @"'Absolute deceleration value less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 for the absolute value of the deceleration (PTP and CNC) is not allowed, since the absolute value of the deceleration is positive by definition, and an absolute value of the deceleration of 0.0 will not allow a motion to be generated."},
            {16945, @"'Set velocity less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 or outside the range from 10-3 up to 10+10 for the set velocity (PTP and CNC) is not allowed, since the set velocity is by definition strictly positive, and with a set velocity of 0.0, division will generate an FPU exception."},
            {16946, @"'Loss of precision when trying a positioning' The positioning is so long in space or time that decimal parts loose there relevance LOSS_OF_PRECISION)."},
            {16947, @"'Cycle time less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 for the cycle time (PTP and CNC) is not allowed, since the cycle time is by definition strictly positive, and with a cycle time of 0.0, division will generate an FPU exception."},
            {16948, @"'PTP data type <intasdouble> range exceeded' Such extreme parameters have been supplied for the start task, the override or the new target position that the internal data type loses its precision."},
            {16949, @"'PTP LHL velocity profile cannot be generated' (INTERNAL ERROR) Such extreme parameters have been supplied for the start task, the override or the new target position that it is not possible to generate a velocity profile of the type LHL (Low-High-Low)."},
            {16950, @"'PTP HML velocity profile cannot be generated' (INTERNAL ERROR) Such extreme parameters have been supplied for the override or the new target position that it is not possible to generate a velocity profile of the type HML (High-Middle-Low)."},
            {16951, @"'Start data address is invalid' The address of the start data is invalid."},
            {16952, @"'Velocity override (start override) is not allowed' The value for the velocity override is not allowed, because it is less than 0.0% or more than 100.0% (see axis interface PlcToNc). Here, 100.0 % corresponds to the integral value 1000000 in the axis interface. Value range: [0 ... 1000000]"},
            {16953, @"'Start type not allowed' The start type supplied does not exist."},
            {16954, @"'Velocity overflow (overshoot in the velocity)' The new dynamic with the parameterized jerk is so weak that a velocity overflow will occur (overshoot in the velocity). The command is therefore not supported."},
            {16955, @"'Start parameter for the axis structure is invalid' External or internal parameters for the start structure for a positioning task are invalid. Thus, for instance, the scaling factor, the SAF cycle time or the requested velocity may be less than or equal to zero, which is not allowed."},
            {16956, @"'Override generator initialization parameter invalid' One of the override generator (re)initialization parameters is invalid."},
            {16957, @"'Slave axis has not set value generator' (INTERNAL ERROR) It is found that a slave axis within a group does not have a valid slave generator (set value generator). A slave axis and a slave set value generator must always be present as a pair. This is an internal error."},
            {16958, @"'Table is empty' Either the SVB table or the SAF table does not contain any entries."},
            {16959, @"'Table is full' The SVB table or the SAF table has no more free lines."},
            {16960, @"'No memory available' SVB memory allocation for dynamic entry in SAF table failed."},
            {16961, @"'Table already contains an entry' (INTERNAL ERROR) SAF table entry abandoned, because, incorrectly, an entry already exists."},
            {16962, @"'Stop is already active' The stop instruction is not forwarded, because it has already been activated."},
            {16963, @"'Compensation has not been carried out over the full compensation section' The compensations start parameters do not permit compensation over the full section to be compensated. For this reason the compensation will be carried out over a smaller section."},
            {16964, @"'Internal parameters for the compensation are invalid' (INTERNAL ERROR) Invalid internal parameters or start parameters of the lower-level generator."},
            {16965, @"'Compensation active' Start of compensation refused, because compensation is already active. It's also possible that the M/S axes are not active moved. Therefore an execution of the compensation is impossible."},
            {16966, @"'Compensation not active' Stop of compensation refused, because compensation is not active."},
            {16967, @"'Compensation type invalid' The type supplied for the section compensation is invalid. At the present time only compensation type 1 (trapezoidal velocity profile) is allowed."},
            {16968, @"'Axis address for compensation invalid' (INTERNAL ERROR) The address of the master of slave axis on which the section compensation is to act is invalid. This is an internal error."},
            {16969, @"'Invalid slave address' (INTERNAL ERROR) The slave address given for on-line coupling/decoupling is invalid."},
            {16970, @"'Coupling velocity invalid' The velocity of what is to become the master axis is 0, which means that on-line coupling is not possible."},
            {16971, @"'Coupling velocities not constant' The velocity of what is to become the master axis and the velocity of what is to become the slave axis are not constant, so that on-line coupling is not possible."},
            {16972, @"'Cycle time less than or equal to 0.0 is not allowed' A value less than or equal to 0.0 for the cycle time (Slave) is not allowed, since the cycle time is by definition strictly positive, and with a cycle time of 0.0, division will generate an FPU exception."},
            {16973, @"'Decoupling task not allowed' The slave axis is of such a type (e.g. a table slave) or is in such a state (master velocity 0) that on-line decoupling is not possible."},
            {16974, @"'Function not allowed' The function cannot logically be executed, e.g. some commands are not possible and not allowed for slave axes."},
            {16975, @"'No valid table weighting has been set' The weighting factor of each table is 0, so that no table can be read."},
            {16976, @"'Axis type, actual position type or end position type is not allowed' The start type for a positioning task in invalid. Valid start types are ABSOLUTE (1), RELATIVE (2), CONTINUOUS POSITIVE (3), CONTINUOUS NEGATIVE (4), MODULO (5), etc. It is also possible that the types for setting a new actual position or for travel to a new end position are invalid."},
            {16977, @"'Function not presently supported' An NC function has been activated that is currently not released for use, or which is not even implemented. This can be a command which is not possible or not allowed for master axes."},
            {16978, @"'State of state machine invalid' (INTERNAL ERROR) The state of an internal state machine is invalid. This is an internal error."},
            {16979, @"'Reference cam became free too soon' During the referencing process for an axis it is moved in the direction of the referencing cam, and is only stopped again when the cam signal is reached. After the axis has then also physically stopped, the referencing cam must remain occupied until the axis subsequently starts back down from the cam in the normal way."},
            {16980, @"'Clearance monitoring between activation of the hardware latch and appearance of the sync pulse' When the clearance monitoring is active, a check is kept on whether the number of increments between activation of the hardware latch and occurrence of the sync pulse (zero pulse) has become smaller than a pre-set value. This error is generated when that happens. (See parameters for the incremental encoder)"},
            {16981, @"'No memory available' The dynamic memory allocation for the set value generator, the SVB table or the SAF table has failed."},
            {16982, @"'The table slave axis has no active table' Although the table slave axis has tables, none of the tables is designated as active. If this occurs during the run time the whole master/slave group is stopped by a run time error."},
            {16983, @"'Function not allowed' The requested function or the requested task is not logically allowed. An example for such an error message would be 'set an actual position' for an absolute encoder (M3000, KL5001, etc.)."},
            {16984, @"'Stopping compensation not allowed' It is not possible to stop the compensation, since compensation is already in the stopping phase."},
            {16985, @"'Slave table is being used' The slave table cannot be activated, because it is currently being used."},
            {16986, @"'Master or slave axis is processing a job (e.g. positioning command) while coupling is requested' A master/slave coupling of a certain slave type (e.g. linear coupling) cannot be executed. he master or intended slave axis is not in stand still state and is executing a job (e.g. positioning) at the same time as the coupling request received. For this couple type this is not allowed."},
            {16987, @"'Slave (start) parameter is incorrect' One of the slave start/coupling parameters is not allowed (Coupling factor is zero, the master position scaling of an cam is zero, etc.)."},
            {16988, @"'Slave type is incorrect' The slave type does not match up to the (SVB) start type."},
            {16989, @"'Axis stop is already active' The axis stop/Estop is not initiated, because the stop/estop is already active."},
            {16990, @"'Maximum number of tables per slavegenerator reached' The maximum number of tables per slave generator is reached (e.g. 'MC_MultiCamIn' is limited to 4 tables)."},
            {16991, @"'The scaling mode is invalid'. The used scaling is invalid in this context. Either the mode is not defined or yet not implemented or however it cannot in this constellation be put into action.	
                    For example MASTER-AUTOOFFSET cannot be used when a cam table is coupled in relative mode because this is a contradiction.	
                    Further MASTER-AUTOOFFSET cannot be used when a cam table is coupled for the first time because a relationship to an existing reference table is missing.   "},
            {16992, @"'Controller enable' Controller enable for the axis or for a coupled slave axis is not present (see axis interface PlcToNc). This error occurs if the controller enable is withdrawn while an axis or a group of axes (also a master/slave group) is being actively positioned. The error also occurs if a PTP axis or a coupled slave axis is started without controller enable."},
            {16993, @"'Table not found' No table exists with the ID prescribed or the table ID is not unique."},
            {16994, @"'Incorrect table type' The table referred to in the function is of the incorrect type."},
            {16995, @"'Single step mode' This error occurs if single step mode is selected for a group or axis and a new task is requested while one of the individual tasks is still being processed."},
            {16996, @"'Group task unknown (asynchronous table entry)' The group has received a task whose type or sub-type is unknown. Valid tasks can be single or multi-dimensional positioning tasks (Geo 1D, Geo 3D), referencing tasks, etc."},
            {16997, @"'Group function unknown (synchronous function)' The group has received a function whose type is unknown. Valid functions are 'Reset', 'Stop', 'New end position', 'Start/stop section compensation', 'Set actual position', 'Set/reset referencing status' etc."},
            {16998, @"'Group task for slave not allowed' Group tasks are usually only possible for master axes, not for slave axes. A slave axis only moves as an indirect result of a positioning task given to its associated master axis. A slave can thus never directly be given a task.
                    Exception: see axis parameter 'Allow motion commands to slave axis'.    "},
            {16999, @"'Group function for slave not allowed' Group functions are in principle only possible for master axes, not for slave axes. The only exception is represented by the 'Start/stop section compensation' function, which is possible both for masters and for slaves. A slave cannot directly execute any other functions beyond this."},
            {17000, @"'NCI Setpoint Generator is Inactive	
                    An NCI command like e.g. 'StopAndKeep' is sent to a logically inactive DXD group or to a group with the state channel override zero.Though, it is expected that for performing this command the NCI group resides actively in setpoint generation. This error can occur related to the functions 'delete distance to go' and 'measurement event (latch actual position)'.	"},
            {17001, @"'Startposition=Setpoint Position' Invalid position parameters."},
            {17002, @"'Parameters of the delay-generator are invalid' Invalid external/internal parameters of the delay generator (delay time, cycle time, tics)."},
            {17003, @"'External parameters of the superimposed instruction are invalid' Invalid external parameters of the superimposed functionality (acceleration, deceleration, velocity, process velocity, length)."},
            {17004, @"'Invalid override type.'"},
            {17005, @"'Activation position under/overrun' The requested activation position is located in the past of the master (e.g. when exchanging a cam table)."},
            {17006, @"'Activation impossible: Master is standing' The required activation of the correction is impossible since the master axis is not moving. A synchronization is not possible, because the master axis standing and the slave axis is still not synchronous."},
            {17007, @"'Activation mode not possible' The requested activation mode is not possible when the slave axis is moving. Otherwise the slave velocity would jump to zero."},
            {17008, @"'Start parameter for the compensation is invalid' One of the dynamic parameters for the compensation is invalid (necessary condition): Acceleration (>0) Deceleration (>0) Process velocity (>0)"},
            {17009, @"'Start parameter for the compensation is invalid' Velocity camber is negative."},
            {17010, @"'Start parameter for the compensation is invalid' The section on which the compensation is to occur is not positive."},
            {17011, @"'Target position under/overrun' (INTERNAL ERROR) The position (calculated from the modulo-target-position) where the axis should stand at end of oriented stop has been run over."},
            {17012, @"'Target position will be under/overrun' (INTERNAL ERROR) The position (calculated from the modulo-target-position) where the axis should stand at end of oriented stop is too near and will be run over."},
            {17013, @"'Group Parameter is Invalid	
                    A group parameter is invalid.In this connection it may be e.g.a parameterized velocity, acceleration, deceleration, jerk or NC cycle time whose value has been parameterized smaller than or equal to zero.	"},
            {17014, @"'Joint Error at Start of Setpoint Generation	
                    At start of setpoint generation for e.g.a flying saw different parameters or states may lead to this error.E.g.dynamic parameters as acceleration, deceleration and jerk may be invalid (smaller than or equal to zero) or the NC cycle time or the override value may reside apart from the interval 0% to 100%.	"},
            {17015, @"'Dynamic parameters not permitted' (INTERNAL ERROR) The dynamic parameters resulting from internal calculation like acceleration, deceleration and jerk are not permitted."},
            {17017, @"'The New Target Position is Invalid or Cannot be Reached	
                    A new commanded target position is invalid because it has already been gone through or will be gone through while stopping with the currently active dynamic.   "},
            {17018, @"'New Velocity for Moving or the Final Target Velocity is Invalid	
                    For a newly commanded command the demanded moving velocity or the demanded final velocity (target velocity in the target position) is invalid.The moving velocity has to be greater than zero value and the final target velocity has always to be greater than or equal to zero(default case is zero value).	"},
            {17019, @"'The Final Velocity or the New Target Position is Invalid	
                    For a newly commanded command the demanded final velocity(target velocity in the target position) or the demanded target position is invalid.The final velocity has to be greater than or equal to zero(default case is zero value).	"},
            {17020, @"'The New Moving Velocity is Invalid	
                    The newly commanded moving velocity is invalid because it is smaller than or equal to zero or other reasons do not facilitate this velocity.    "},
            {17021, @"'Internal Starting Mode is Invalid	
                    For a newly commanded command this starting mode is invalid or is not permitted within this situation of movement.The user cannot influence the starting mode directly.	"},
            {17022, @"'A requested motion command could not be realized (BISECTION)' A requested motion command could not be realized using the requested parameters.The command has been executed best possible and this message is therefore to be understood just as a warning. Samples:	
                    An axis motion command is requested while the axis is in a unfavorable dynamic situation (acceleration phase), in which the covered distance is too short or the velocity is clearly too high.Another possibility is a slave axis, which is decoupled in motion in an unfavorable dynamic situation and is afterwards given a motion as in the previous case.	"},
            {17023, @"'The new target position either has been overrun or will be overrun' The new target position either has been overrun or will be overrun, since until there it is impossible to stop. An internal stop command is commended."},
            {17024, @"'Group not ready / group not ready for new task' (INTERNAL ERROR / INFORMATION) The group is being given a new task while it is still in the process of executing an existing task. This request is not allowed because it would interrupt the execution of the previous task. The new task could, for instance, be a positioning command, or the 'set actual position' function. Precisely the converse relationships apply for the 'set new end position' function. In that case, the group/axis must still be actively moving in order to be able to cause a change in the end position."},
            {17025, @"'The parameters of the oriented stop (O-Stop) are not admitted.' The modulo-target position should not be smaller than zero and not larger or equal than the encoder mod-period ( e.g. in the interval [0.0,360.0] ).	
                    Note: In the case of error the axis is safely stopped, but is afterwards not at the requested oriented position.    "},
            {17026, @"'The modulo target position of the modulo-start is invalid' The modulo target position is outside of the valid parameter range. So the position value should not be smaller than zero and not greater or equal than the encoder modulo-period (e. g. in the interval [0.0,360.0] for the modulo start type 'SHORTEST_WAY (261)' )."},
            {17027, @"'The online change activation mode is invalid'.The activation can be used with online scaling or with online modification of motion function. The used activation is invalid in this context. Either the mode is not defined or yet not implemented or however it cannot in this constellation be put into action (e.g. when linear tables are used with an unexpected cyclic activation mode NEXTCYCLE or NEXTCYCLEONCE).	
                    In some case, the activation mode may be valid but the command cannot be executed due to a pending previous command.	"},
            {17028, @"'The parameterized jerk rate is not permitted'. The jerk rate is smaller than the minimum jerk rate. The minimum value for jerk rate is 1.0 (e.g. mm/s^3)."},
            {17029, @"'The parameterized acceleration or deceleration is not permitted'. The parameterized acceleration or deceleration is lower than the permitted minimum acceleration. The value for minimum acceleration is calculated from minimum jerk rate and NC cycle time (minimum jerk rate multiplied with NC cycle time). The unit for example is mm/s^2."},
            {17030, @"'The parameterized velocity is not permitted'. The parameterized target velocity is lower than the minimum velocity (but the value zero is permitted). The value for minimum velocity is calculated from the minimum jerk rate and the NC cycle time (minimum jerk rate multiplied with the square of the NC cycle time). The unit for example is mm/s."},
            {17031, @"'A activation cannot be executed due to a pending activation' A activation e.g. 'CamIn', 'CamScaling' or 'WriteMotionFunction' cannot be executed due to a pending activation (e.g. 'CamIn', 'CamScaling', 'WriteMotionFunction'). Only activation can be enabled."},
            {17032, @"'Illegal combination of different cycle times within an axis group' A logical axis group includes elements (axes) with different cycle times for a common setpoint generator and I/O-execution, resp. This situation can occur with Master/Slave-coupling or configuring 3D- and FIFO-groups (including path, auxiliary, and slave axes)."},
            {17033, @"'Illegal motion reversal' Due to the actual dynamical state a motion reversal will happen. To avoid this motion reversal the axis command is not performed and the previous system state restored."},
            {17034, @"'Illegal moment for an axis command because there is an old axis command with activation position still active' The moment for the command is illegal because there is still an old command with activation position active (e.g. 'go to new velocity at threshold position' or 'reach new velocity at threshold position')."},
            {17035, @"'Error in the stop-calculation routine' (INTERNAL ERROR) Due to an internal error in the stop-calculation routine the current commando cannot be performed. The previous system state is restored."},
            {17036, @"'A command with activation position cannot fully be performed because the remaining path is too short'A command with activation position (threshold) like 'reach a new velocity at a position' can be just partially executed because the path from the actual position to the activation position is too short."},
            {17037, @"'Illegal decouple type when decoupling a slave axis' The decouple and restart command contains an invalid decouple type."},
            {17038, @"'Illegal target velocity when decoupling a slave axis' The decouple and restart command contains an illegal target velocity [1 < V <Vmax]."},
            {17039, @"'The command new dynamic parameter cannot be performed since this would require a new target velocity'Das Kommando zum Aktivieren neuer Dynamikparameter wie Beschleunigung, Verzögerung und Ruck kann nicht durchgeführt werden, da dies eine neue beauftragte Fahrgeschwindigkeit erfordern würde.
                    This situation can occur, for example, if the axis is near the target position in an accelerated state and the dynamics parameter are chosen softer.    "},
            {17040, @"'A command with activation position cannot be performed because the axis is already in the brake phase' A command with activation position (threshold) e.g. 'reach new velocity at position' cannot be performed because the axis is already in the brake phase and the remaining path from the actual position to the activation position is too short."},
            {17041, @"'Decouple routine of slave axis doesn't return a valid solution' Internal jerk scaling of decouple routine cannot evaluate a valid solution (decoupling slave axis and transform to master axis). The command is rejected because velocity can become too high, a reversal of movement can occur, or the target position can be passed."},
            {17042, @"'Command not be executed because the command buffer is full filled' The command is rejected because the command buffer is full filled."},
            {17043, @"'Command is rejected due to an internal error in the Look Ahead' (INTERNAL ERROR) The command is rejected due to an internal error in the 'look ahead'."},
            {17044, @"'Command is rejected because the segment target velocity is not realized' The command is rejected, because the new target segment velocity Vrequ is not realizable and an internal optimizing is impossible."},
            {17045, @"'Successive commands have the same final position' Successive commands have the same final position. So the moving distance is zero."},
            {17046, @"'Logical positioning direction is inconsistent with the direction of the buffer command' In the extended buffer mode, where the actual end position is replaced by the new buffer start position, the logical positioning direction is inconsistent with the direction of the buffer command (=> contradiction). A buffered command (BufferMode, BlendingLow, BlendingPrevious, BlendingNext, BlendingHigh) is rejected with error 0x4296 if the command is using the Beckhoff specific optional BlendingPosition but the blending position is located beyond the target position of the previous motion command."},
            {17047, @"'Command is rejected because the remaining positioning length is to small' The command is rejected because the remaining path length is too small.	
                    E.g.when the buffer mode is used and the remaining positioning length in the actual segment is too small for getting the axis in a force free state or to reach the new target velocity at the change of segment.  "},
            {17050, @"'Restart has Failed	
                    There is already a motion command within the PTP command buffer and a further new motion command that should have modified the current motion command by restart has failed.    "},
            {17051, @"'collect error for invalid start parameters'
                    This error refers to a wrong parameterization of the user (collect error). E.g.dynamic parameters like Velo, Acc or Dec could be equal or less than zero.
                    Or following errors:	
                    - BaseFrequence< 0.0
                    - StartFrequence< 1.0
                    - StepCount< 1, StepCount> 200
                    - BaseAmplitude <= 0.0
                    - StepDuration <= 0.0
                    - StopFrequence >= 1/(2*CycleTime)  "},
            {17052, @"'Reference cam is not found' During the referencing process for an axis it is moved in the direction of the referencing cam. This reference cam, however, was not found as expected (=> leads to the abortion of the referencing procedure)."},
            {17053, @"'Reference cam became not free' During the referencing process for an axis it is moved in the direction of the referencing cam, and is only stopped again when the cam signal is reached. After the axis has also come to a physical standstill, the axis is subsequently started regularly from the cam again. In this case, the reference cam did not become free again as expected when driving down (=> leads to the abortion of the referencing procedure)."},
            {17054, @"'IO sync pulse was not found (only when using hardware latch)' If the hardware latch is activated, a sync pulse (zero pulse) is expected to be found and a sync event triggered following the expiry of a certain time or a certain distance. If this is not the case, the reaction is an error and the abortion of the referencing procedure."},
            {17055, @"'The Used Buffer Mode is Unknown or not Supported in this Context	
                    The buffer mode used for a PTP command(e.g.ABORTING, etc.) is unknown or not supported in this context.   "},
            {17056, @"'Group/axis consequential error' Consequential error resulting from another causative error related to another axis within the group. Group/axis consequential errors can occur in relation to master/slave couplings or with multiple axis interpolating DXD groups. If, for instance, it is detected that the following error limit of a master axis has been exceeded, then this consequential error is assigned to all the other master axes and slave axes in this group."},
            {17057, @"'Velocity reduction factor for C0/C1 transition is not allowed' A C0 transition describes two geometries which, while they are themselves continuous, no not have either continuous first or second differentials. The velocity reduction factor C0 acts on such transitions. Note: A C1 transition is characterized by the two geometries being continuous, but having only a first differential that is continuous. The velocity reduction factor C1 acts on such transitions."},
            {17058, @"'Critical angle at segment transition not allowed'"},
            {17059, @"'Radius of the tolerance sphere' is in an invalid rang"},
            {17060, @"'Not implemented."},
            {17061, @"'Start type'"},
            {17062, @"'Not implemented."},
            {17063, @"'Blending' with given parameters not possible"},
            {17064, @"'Not implemented."},
            {17065, @"'Curve velocity reduction method not allowed' (INTERNAL ERROR) The curve velocity reduction method does not exist."},
            {17066, @"'Minimum velocity not allowed' The minimum velocity that has been entered is less than 0.0."},
            {17067, @"'Power function input not allowed' (INTERNAL ERROR) The input parameters in the power_() function lead to an FPU exception."},
            {17068, @"'Dynamic change parameter not allowed' A parameter that controls alterations to the dynamics is invalid. Parameter: 1. Absolute motion dynamics change: All parameters must be strictly positive. 2. Relative reduction c_f: 0.0 < c_f <= 1.0"},
            {17069, @"'Memory allocation error' (INTERNAL ERROR)"},
            {17070, @"'The calculated end position differs from the end position in the nc instruction (internal error).'"},
            {17071, @"'Calculate remaining chord length'
                    invalid value   "},
            {17072, @"'Set value generator SVB active' Starting the set value generator (SVB, SAF) has been refused, since the SVB task is already active."},
            {17073, @"'SVB parameter not allowed' (INTERNAL ERROR) A parameter related to the internal structure of the set value generator (SVB) results in logical errors and/or to an FPU exception. Affects these parameters: Minimum velocity (>0.0), TimeMode, ModeDyn, ModeGeo, StartType, DistanceToEnd, TBallRadius."},
            {17074, @"'Velocity reduction factor not allowed' A parameter that controls reduction of the velocity at segment transitions is invalid. Parameter: 1. Transitions with continuous first differential: VeloVertexFactorC1 2. Not once continuously differentiable transitions: VeloVertexFactorC0, CriticalVertexAngleLow, CriticalVertexAngleHigh."},
            {17075, @"'Helix is a circle' The helix has degenerated to a circle, and should be entered as such."},
            {17076, @"'Helix is a straight line' The helix has degenerated to a straight line, and should be entered as such."},
            {17077, @"'Guider parameter not allowed' One of the guider's parameters leads to logical errors and/or to an FPU exception."},
            {17078, @"'Invalid segment address' (INTERNAL ERROR) The geometry segment does not have a valid geometry structure address or does not have a valid dynamic structure address."},
            {17079, @"'Unparameterized generator' (INTERNAL ERROR) The SVB generator is not yet parameterized and is therefore unable to operate."},
            {17080, @"'Unparameterized table' (INTERNAL ERROR) The table has no information concerning the address of the corresponding dynamic generator."},
            {17082, @"'The calculation of the arc length of the smoothed path failed (internal error).'"},
            {17083, @"'The radius of the tolerance ball is too small (smaller than 0.1 mm).'"},
            {17084, @"'Error while calculating DXD-Software-Limit switches (internal error)"},
            {17085, @"'NC-Block violates software limit switches of the group' At least one path axis with active software limit monitoring has violated the limit switches. Therefore the geometric entry is denied with an error."},
            {17086, @"'Internal error in the evaluation of a possible software limit switch violation for the segment with the block-number xx.' At least one path axis with active position limit monitoring has violated the limit switches."},
            {17087, @"'Invalid reference speed type."},
            {17088, @"'Interpolating group contains axes of an incorrect axis type' An interpolating 3D group may only contain continuously guided axes of axis type 1 (SERVO)."},
            {17089, @"'Scalar product cannot be calculated' The length of one of the given vectors is 0.0."},
            {17090, @"'Inverse cosine cannot be calculated' The length of one of the given vectors is 0.0."},
            {17091, @"'Invalid table entry type' The given table entry type is unknown."},
            {17092, @"'Invalid DIN66025 information type' (INTERNAL ERROR) The given DIN66025 information type is unknown. Known types: G0, G1, G2, G3, G17, G18, G19."},
            {17093, @"'Invalid dimension' (INTERNAL ERROR) The CNC dimension is unknown. Known dimensions: 1, 2, 3. Or: The CNC dimension is invalid for the given geometrical object. For a circle the dimension must be 2 or 3, while for a helix it must be 3."},
            {17094, @"'Geometrical object is not a straight line' The given object, interpreted as a straight line, has a length of 0.0."},
            {17095, @"'Geometrical object is not a circle' Interpreted as a circular arc, the given object has a length of 0.0, or an angle of 0.0 or a radius of 0.0."},
            {17096, @"'Geometrical object is not a helix' Interpreted as a circular arc, the given object has a length of 0.0, or an angle of 0.0, or a radius of 0.0. or a height of 0.0."},
            {17097, @"'Set velocity less than or equal to 0.0 is invalid' A value less than or equal to 0.0 for the set velocity (CNC) is not allowed, since the set velocity is positive by definition, and a set velocity of 0.0 cannot generate any motion."},
            {17098, @"'Address for look-ahead invalid' (INTERNAL ERROR) The address supplied for the look-ahead is invalid."},
            {17099, @"'Set value generator SAF active' Starting the set value generator (SAF) has been refused, since the SAF task is already active."},
            {17100, @"'CNC set value generation not active' Stop or change of override refused, because the set value generation is not active."},
            {17101, @"'CNC set value generation in the stop phase' Stop or change of override refused, because the set value generation is in the stop phase."},
            {17102, @"'Override not allowed' An override of less than 0.0 % or more than 100.0 % is invalid."},
            {17103, @"'Invalid table address' (INTERNAL ERROR) The table address given for the initialization of the set value generator is invalid, or no valid logger connection (report file) is present."},
            {17104, @"'Invalid table entry type' The given table entry type is unknown."},
            {17105, @"'Memory allocation failed' Memory allocation for the table has failed."},
            {17106, @"'Memory allocation failed' Memory allocation for the filter has failed."},
            {17107, @"'Invalid parameter' Filter parameter is not allowed."},
            {17108, @"'Delete Distance To Go failed' Delete Distance to go (only interpolation) failed. This error occurred, if e.g. the command 'DelDTG' was not programmed in the actual movement of the nc program."},
            {17109, @"'The setpoint generator of the flying saw generates incompatible values (internal error)'"},
            {17110, @"'Axis will be stopped since otherwise it will overrun its target position (old PTP setpoint generator)' If, for example, in case of a slave to master transformation for the new master a target position is commanded that will be overrun because of the actual dynamics the axis will be stopped internally to guarantee that the target position will not be overrun."},
            {17111, @"'Internal error in the transformation from slave to master.'"},
            {17112, @"'Wrong direction in the transformation of slave to master.'"},
            {17114, @"'Parameter of Motion Function (MF) table incorrect' The parameter of the Motion Function (MF) are invalid. This may refer to the first time created data set or to online changed data."},
            {17115, @"'Parameter of Motion Function (MF) table incorrect' The parameter of the Motion Function (MF) are invalid. This may refer to the first time created data set or to online changed data.
                    The error cause can be, that an active MF point(no IGNORE point) points at a passive MF point(IGNORE point).    "},
            {17116, @"'Internal error by using Motion Function (MF)' An internal error occurs by using the Function (MF). This error cannot be solved by the user. Please ask the TwinCAT Support."},
            {17117, @"'Axis coupling with synchronization generator declined because of incorrect axis dynamic values' The axis coupling with the synchronization generator has been declined, because one of the slave dynamic parameter (machine data) is incorrect. Either the maximum velocity, the acceleration, the deceleration or the jerk is smaller or equal to zero, or the expected synchronous velocity of the slave axis is higher as the maximum allowed slave velocity."},
            {17118, @"'Coupling conditions of synchronization generator incorrect' During positive motion of the master axis it has to be considered, that the master synchronous position is larger than the master coupling position ('to be in the future'). During negative motion of the master axis it has to be considered that the master synchronous position is smaller than the master coupling position."},
            {17119, @"'Moving profile of synchronization generator declines dynamic limit of slave axis or required characteristic of profile' One of the parameterized checks has recognized an overstepping of the dynamic limits (max. velocity, max. acceleration, max. deceleration or max. jerk) of the slave axis, or an profile characteristic (e.g. overshoot or undershoot in the position or velocity) is incorrect.
                    See also further messages in the windows event log and in the message window of the System Manager. "},
            {17120, @"'Invalid parameter' The encoder generator parameter is not allowed."},
            {17121, @"'Invalid parameter' The external (Fifo) generator parameter is not allowed."},
            {17122, @"'External generator is active' The external generator cannot be started, as it is already active."},
            {17123, @"'External generator is not active' The external generator cannot be stopped, as it is not active."},
            {17124, @"'NC-Block with auxiliary axis violates software limit switches of the group' At least one auxiliary axis with active software limit monitoring has violated the limit switches. Therefore the geometric entry is denied with an error."},
            {17125, @"'NC-Block type Bezier spline curve contains a cusp (singularity)' The Bezier spline curve contain a cusp, i.e.at a certain interior point both the curvature and the modulus of the velocity tend to 0 such that the radius of curvature is infinite.
                    Note: Split the Bezier curve at that point into two Bezier spline curves according to the de 'Casteljau algorithm'. This preserves the geometry and eliminates the interior singularity.    "},
            {17127, @"'Value for dead time compensation not allowed' The value for the dead time compensation in seconds for a slave coupling to an encoder axis (virtual axis) is not allowed."},
            {17128, @"'GROUPERR_RANGE_NOMOTIONWINDOW'"},
            {17129, @"'GROUPERR_RANGE_NOMOTIONFILTERTIME'"},
            {17130, @"'GROUPERR_RANGE_TIMEUNITFIFO'"},
            {17131, @"'GROUPERR_RANGE_OVERRIDETYPE'"},
            {17132, @"'GROUPERR_RANGE_OVERRIDECHANGETIME'"},
            {17133, @"'GROUPERR_FIFO_INVALIDDIMENSION'	
                    Note: Since TC 2.11 Build 1547 the FIFO-dimension(number of axes) has been increased from 8 to 16.	"},
            {17134, @"'GROUPERR_ADDR_FIFOTABLE'"},
            {17135, @"'Axis is locked for motion commands because a stop command is still active' The axis/group is locked for motion commands because a stop command is still active. The axis can be released by calling MC_Stop with Execute=FALSE or by using MC_Reset (TcMC2.Lib)."},
            {17136, @"'Invalid number of auxiliary axes' The local number of auxiliary axes does not tally with the global number of auxiliary axes."},
            {17137, @"'Invalid reduction parameter for auxiliary axes' The velocity reduction parameters for the auxiliary axes are inconsistent."},
            {17138, @"'Invalid dynamic parameter for auxiliary axes' The dynamic parameters for the auxiliary axes are inconsistent."},
            {17139, @"'Invalid coupling parameter for auxiliary axes' The coupling parameters for the auxiliary axes are inconsistent."},
            {17140, @"'Invalid auxiliary axis entry' The auxiliary axis entry is empty (no axis motion)."},
            {17142, @"'Invalid parameter' The limit for velocity reduction of the auxiliary axes is invalid. It has to be in the interval 0..1.0"},
            {17144, @"'Block search - segment not found' The segment specified as a parameter could not be found by the end of the NC program.
                    Possible cause:		
                    nBlockId is not specified in the mode described by eBlockSearchMode	
	                    "},
            {17145, @"'Blocksearch – invalid remaining segment length' The remaining travel in the parameter fLength is incorrectly parameterized"},
            {17147, @"'Internal Error in the Context of Coupled Axes (Slave Axes)	
                    Fatal internal error using coupled axes(slave axes). Inconsistent internal state.Please, contact the support team.    "},
            {17148, @"'Parameter for the Maximum Number of Jobs (Entries) to be Transferred is Invalid	
                    The parameter that describes the maximum number of entries to transfer from the SVB to the SAF table per NC Cycle is invalid.   "},
            {17151, @"'Customer Specific Error	
                    In this connection it is about a customer specific monitoring function.	"},
            {17152, @"'Axis ID not allowed' The value for the axis ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, is greater than 255, or does not exist in the current configuration."},
            {17153, @"'Axis type not allowed' The value for the axis type is unacceptable because it is not defined. Type 1: Servo Type 2: Fast/creep Type 3: Stepper motor"},
            {17158, @"'Slow manual velocity not allowed' The value for the slow manual velocity is not allowed."},
            {17159, @"'Fast manual velocity not allowed' The value for the fast manual velocity is not allowed."},
            {17160, @"'High speed not allowed' The value for the high speed is not allowed."},
            {17161, @"'Acceleration not allowed' The value for the axis acceleration is not allowed."},
            {17162, @"'Deceleration not allowed' The value for the axis deceleration is not allowed."},
            {17163, @"'Jerk not allowed' The value for the axis jerk is not allowed."},
            {17164, @"'Delay time between position and velocity is not allowed' The value for the delay time between position and velocity ('idle time compensation') is not allowed."},
            {17165, @"'Override-Type not allowed' The value for the velocity override type is not allowed. Type 1: With respect to the internal reduced velocity (default value) Type 2: With respect to the original external start velocity"},
            {17166, @"'NCI: Velo-Jump-Factor not allowed'
                    The value for the velo-jump-factor ('VeloJumpFactor') is not allowed.This parameter only works for TwinCAT NCI.	"},
            {17167, @"'NCI: Radius of tolerance sphere for the auxiliary axes is invalid'	
                    It was tried to enter an invalid value for the size of the tolerance sphere. This sphere affects only auxiliary axes!	"},
            {17168, @"'NCI: Value for maximum deviation for the auxiliary axes is invalid'	
                    It was tried to enter an invalid value for the maximum allowed deviation. This parameter affects only auxiliary axes!	"},
            {17170, @"'Referencing velocity in direction of cam not allowed' The value for the referencing velocity in the direction of the referencing cam is not allowed."},
            {17171, @"'Referencing velocity in sync direction not allowed' The value for the referencing velocity in the direction of the sync pulse (zero track) is not allowed."},
            {17172, @"'Pulse width in positive direction not allowed' The value for the pulse width in the positive direction is not allowed (pulsed operation). The use of the pulse width for positioning is chosen implicitly through the axis start type. Pulsed operation corresponds to positioning with a relative displacement that corresponds precisely to the pulse width."},
            {17173, @"'Pulse width in negative direction not allowed' The value for the pulse width in the negative direction is not allowed (pulsed operation). The use of the pulse width for positioning is chosen implicitly through the axis start type. Pulsed operation corresponds to positioning with a relative displacement that corresponds precisely to the pulse width."},
            {17174, @"'Pulse time in positive direction not allowed' The value for the pulse width in the positive direction is not allowed (pulsed operation)."},
            {17175, @"'Pulse time in negative direction not allowed' The value for the pulse width in the negative direction is not allowed (pulsed operation)."},
            {17176, @"'Creep distance in positive direction not allowed' The value for the creep distance in the positive direction is not allowed."},
            {17177, @"'Creep distance in negative direction not allowed' The value for the creep distance in the negative direction is not allowed."},
            {17178, @"'Braking distance in positive direction not allowed' The value for the braking distance in the positive direction is not allowed."},
            {17179, @"'Braking distance in negative direction not allowed' The value for the braking distance in the negative direction is not allowed."},
            {17180, @"'Braking time in positive direction not allowed' The value for the braking time in the positive direction is not allowed."},
            {17181, @"'Braking time in negative direction not allowed' The value for the braking time in the negative direction is not allowed."},
            {17182, @"'Switching time from high to low speed not allowed' The value for the time to switch from high to low speed is not allowed."},
            {17183, @"'Creep distance for stop not allowed' The value for the creep distance for an explicit stop is not allowed."},
            {17184, @"'Motion monitoring not allowed' The value for the activation of the motion monitoring is not allowed."},
            {17185, @"'Position window monitoring not allowed' The value for the activation of the position window monitoring is not allowed."},
            {17186, @"'Target window monitoring not allowed' The value for the activation of target window monitoring is not allowed."},
            {17187, @"'Loop not allowed' The value for the activation of loop movement is not allowed."},
            {17188, @"'Motion monitoring time not allowed' The value for the motion monitoring time is not allowed."},
            {17189, @"'Target window range not allowed' The value for the target window is not allowed."},
            {17190, @"'Position window range not allowed' The value for the position window is not allowed."},
            {17191, @"'Position window monitoring time not allowed' The value for the position window monitoring time is not allowed."},
            {17192, @"'Loop movement not allowed' The value for the loop movement is not allowed."},
            {17193, @"'Axis cycle time not allowed' The value for the axis cycle time is not allowed."},
            {17194, @"'Stepper motor operating mode not allowed' The value for the stepper motor operating mode is not allowed."},
            {17195, @"'Displacement per stepper motor step not allowed' The value for the displacement associated with one step of the stepper motor is not allowed (step scaling)."},
            {17196, @"'Minimum speed for stepper motor set value profile not allowed' The value for the minimum speed of the stepper motor speed profile is not allowed."},
            {17197, @"'Stepper motor stages for one speed stage not allowed' The value for the number of steps for each speed stage in the set value generation is not allowed."},
            {17198, @"'DWORD for the interpretation of the axis units not allowed' The value that contains the flags for the interpretation of the position and velocity units is not allowed."},
            {17199, @"'Maximum velocity not allowed' The value for the maximum permitted velocity is not allowed."},
            {17200, @"'Motion monitoring window not allowed' The value for the motion monitoring window is not allowed."},
            {17201, @"'PEH time monitoring not allowed' The value for the activation of the PEH time monitoring is not allowed (PEH: positioning end and halt)."},
            {17202, @"'PEH monitoring time not allowed' The value for the PEH monitoring time (timeout) is not allowed (PEH: positioning end and halt). default value: 5s"},
            {17203, @"'Parameter 'Break Release Delay' is Invalid"},
            {17204, @"'Parameter 'NC Data Persistence' is Invalid"},
            {17205, @"'Parameter for the Error Reaction Mode is Invalid"},
            {17206, @"'Parameter for the Error Reaction Delay is Invalid	
                    The parameter for the error reaction delay of the axis is invalid.  "},
            {17207, @"'Parameter 'Couple Slave to Actual Values if not Enabled' is Invalid	
                    The parameter 'Couple Slave to Actual Values if not Enabled' is invalid.    "},
            {17208, @"'Parameter 'Allow Motion Commands to Slave Axis' is Invalid	
                    The boolean parameter 'Allow Motion Commands to Slave Axis' is invalid.This parameter defines whether a motion command can be sent to a slave axis or whether this is rejected with the NC error 0x4266 or 0x4267.	"},
            {17209, @"'Parameter 'Allow Motion Commands to External Setpoint Axis' is Invalid	
                    The boolean parameter 'Allow Motion Commands to External Setpoint Axis' is invalid.This parameter defines whether a motion command may be send to an axis within the state of external setpoint generation or whether such a message is rejected with error 0x4257.	"},
            {17210, @"'Parameter 'Fading Acceleration' is Invalid	
                    The parameter 'Fading Acceleration' for the blending profile from set to actual values is invalid.This parameter defines how to blend from a setpoint based axis coupling to an actual value based coupling (indirectly there is a time for the blending).	
                    Note: The value 0.0 causes that the minimum of default acceleration and default deceleration is used as blending acceleration internally within the NC. "},
            {17211, @"'Fast Axis Stop Signal Type not allowed' The value for the Signal Type of the 'Fast Axis Stop' is not allowed [0...5]."},
            {17216, @"'Axis initialization' Axis has not been initialized. Although the axis has been created, the rest of the initialization has not been performed (1. Initialization of axis I/O, 2. Initialization of axis, 3. Reset axis)."},
            {17217, @"'Group address' Axis does not have a group, or the group address has not been initialized (group contains the set value generation)."},
            {17218, @"'Encoder address' The axis does not have an encoder, or the encoder address has not been initialized."},
            {17219, @"'Controller address' The axis does not have a controller, or the controller address has not been initialized."},
            {17220, @"'Drive address' The axis does not have a drive, or the drive address has not been initialized."},
            {17221, @"'Axis interface PLC to NC address' Axis does not have an axis interface from the PLC to the NC, or the axis interface address has not been initialized."},
            {17222, @"'Axis interface NC to PLC address' Axis does not have an axis interface from the NC to the PLC, or the axis interface address has not been initialized."},
            {17223, @"'Size of axis interface NC to PLC is not allowed' (INTERNAL ERROR) The size of the axis interface from NC to PLC is not allowed."},
            {17224, @"'Size of axis interface PLC to NC is not allowed' (INTERNAL ERROR) The size of the axis interface from PLC to NC is not allowed."},
            {17238, @"'Controller enable' Controller enable for the axis is not present (see axis interface SPS®NC). This enable is required, for instance, for an axis positioning task."},
            {17239, @"'Feed enable negative: There is no feed enable for negative motion direction (see axis interface PLC->NC). This enable is checked e.g. for a positioning task of an axis into negative motion direction."},
            {17240, @"'Feed enable plus' Feed enable for movement in the positive direction is not present (see axis interface SPS®NC). This enable is required, for instance, for an axis positioning task in the positive direction."},
            {17241, @"'Set velocity not allowed' The set velocity requested for a positioning task is not allowed. This can happen if the velocity is less than or equal to zero, larger than the maximum permitted axis velocity, or, in the case of servo-drives, is larger than the reference velocity of the axis (see axis and drive parameters)."},
            {17242, @"'Movement smaller than one encoder increment' (INTERNAL ERROR) The movement required of an axis is, in relation to a positioning task, smaller than one encoder increment (see scaling factor). This information is, however, handled internally in such a way that the positioning is considered to have been completed without an error message being returned."},
            {17243, @"'Set acceleration monitoring' (INTERNAL ERROR) The set acceleration has exceeded the maximum permitted acceleration or deceleration parameters of the axis."},
            {17244, @"'PEH time monitoring' The PEH time monitoring has detected that, after the PEH monitoring time that follows a positioning has elapsed, the target position window has not been reached. The following points must be checked: Is the PEH monitoring time, in the sense of timeout monitoring, set to a sufficiently large value (e.g. 1-5 s)? The PEH monitoring time must be chosen to be significantly larger than the target position monitoring time. Have the criteria for the target position monitoring (range window and time) been set too strictly? Note: The PEH time monitoring only functions when target position monitoring is active!"},
            {17245, @"'Encoder existence monitoring / movement monitoring' During the active positioning the actual encoder value has changed continuously for a default check time from NC cycle to NC cycle less than the default minimum movement limit. => Check, whether axis is mechanically blocked, or the encoder system failed, etc... Note: The check is not performed while the axis is logically standing (position control), but only at active positioning (it would make no sense if there is a mechanical holding brake at the standstill)!"},
            {17246, @"'Looping distance less than breaking distance' The absolute value of the looping distance is less or equal than the positive or negative breaking distance. This is not allowed."},
            {17247, @"'Starting Velocity Invalid
                    The required starting velocity for a positioning task is not permitted (usually the starting velocity is zero). This situation can occur if the velocity is smaller than or equal to zero, greater than the axis maximum permitted velocity or for servo motion controllers greater than the axis reference velocity(see axis and motion controller parameters)."},
            {17248, @"'Final Velocity Invalid"},
            {17249, @"'Time range exceeded (future)' The calculated position lies too far in the future (e.g. when converting a position value in a DC time stamp)."},
            {17250, @"'Time range exceeded (past)' The calculated position lies too far in the past (e.g. when converting a position value in a DC time stamp)."},
            {17251, @"'Position cannot be determined' The requested position cannot be determined. Case 1: It was not passed through in the past. Case 2: It cannot be reached in future. A reason can be a zero velocity value or an acceleration that causes a turn back."},
            {17252, @"'Position indeterminable (conflicting direction of travel)' The direction of travel expected by the caller of the function deviates from the actual direction of travel (conflict between PLC and NC view, for example when converting a position to a DC time)."},
            {17264, @"'No Slave Coupling Possible (Velocity Violation)
                    A slave coupling to a master axis(e.g.by a universal flying saw) is rejected because otherwise the maximum velocity of the slave axis would be exceeded(a velocity monitoring has been selected)."},
            {17265, @"'No Slave Coupling Possible (Acceleration Violation)
                    A slave coupling to a master axis(e.g.by a universal flying saw) is rejected, because otherwise the maximum acceleration of the slave axis will be exceeded(an acceleration monitoring is selected)."},
            {17266, @"'The synchronization profile would violate the lower end position of the slave.
                    Check when GearInSync_CheckMask_MinPos is active."},
            {17267, @"'The synchronization profile would violate the upper end position of the slave.
                    Check when GearInSync_CheckMask_MaxPos is active."},
            {17268, @"'The synchronization profile would violate the user limit position Options.PositionLimitMin.
                    Check when GearInSync_CheckMask_OptionalMinPos is active."},
            {17269, @"'The synchronization profile would violate the user limit position Options.PositionLimitMax.
                    Check when GearInSync_CheckMask_OptionalMaxPos is active."},
            {17270, @"'The synchronization point lies under the starting point. As a result, the profile swings under both the start position and the synchronous position.
                    Check when GearInSync_CheckMask_UndershootPos is active."},
            {17271, @"'The synchronization profile would swing back under the slave start position of the flying saw.
                    Check when GearInSync_CheckMask_UndershootPos is active."},
            {17272, @"'The synchronization point lies under the starting point. The synchronization profile would swing beyond the slave start position of the flying saw.
                    Check when GearInSync_CheckMask_OvershootPos is active."},
            {17273, @"'The synchronization profile would swing beyond the slave synchronous position of the flying saw.
                    Check when GearInSync_CheckMask_OvershootPos is active."},
            {17274, @"'The maximum velocity of the synchronization profile is higher than the maximum velocity of the slave axis.
                    Check when GearInSync_CheckMask_MaxVelo is active."},
            {17275, @"'The maximum velocity of the synchronization profile is higher than the maximum velocity of the slave axis.
                    Check when GearInSync_CheckMask_MaxVelo is active."},
            {17276, @"'The maximum velocity of the synchronization profile would be higher than the synchronous velocity.
                    (Positive direction of travel and starting velocity lower than synchronous velocity)
                    Check when GearInSync_CheckMask_OvershootVelo is active."},
            {17277, @"'The maximum velocity of the synchronization profile would be higher than the starting velocity and synchronous velocity.
                    (Positive direction of travel and starting velocity higher than synchronous velocity)
                    Check when GearInSync_CheckMask_OvershootVelo is active."},
            {17278, @"'The maximum velocity of the synchronization profile would be higher than the synchronous velocity.
                    (Negative direction of travel and starting velocity lower than synchronous velocity)
                    Check when GearInSync_CheckMask_OvershootVelo is active"},
            {17279, @"'The maximum velocity of the synchronization profile would be higher than the starting velocity and synchronous velocity.
                    (Negative direction of travel and starting velocity higher than synchronous velocity)
                    Check when GearInSync_CheckMask_OvershootVelo is active."},
            {17280, @"'The minimum velocity of the synchronization profile lies below the synchronous velocity.
                    (Positive direction of travel and starting velocity higher than synchronous velocity)
                    Check when GearInSync_CheckMask_UndershootVelo is active."},
            {17281, @"'The minimum velocity of the synchronization profile lies below the starting velocity.
                    (Positive direction of travel and starting velocity lower than synchronous velocity)
                    Check when GearInSync_CheckMask_UndershootVelo is active."},
            {17282, @"'The minimum velocity of the synchronization profile lies below the synchronous velocity.
                    (Negative direction of travel and starting velocity higher than synchronous velocity)
                    Check when GearInSync_CheckMask_UndershootVelo is active."},
            {17283, @"'The minimum velocity of the synchronization profile lies below the starting velocity.
                    (Negative direction of travel and starting velocity lower than synchronous velocity)
                    Check when GearInSync_CheckMask_UndershootVelo is active."},
            {17286, @"'The velocity of the flying saw swings below zero; the motion is reversed.
                    If the slave is already moving in the opposite direction at the beginning, it is not regarded as UndershootVeloZero.
                    (Master moves in the positive direction)
                    Check when GearInSync_CheckMask_UndershootVeloZero is active."},
            {17287, @"'The velocity of the flying saw swings below zero; the motion is reversed.
                    If the slave is already moving in the opposite direction at the beginning, it is not regarded as UndershootVeloZero.
                    (Master moves in the negative direction)
                    Check when GearInSync_CheckMask_UndershootVeloZero is active."},
            {17288, @"'The maximum acceleration of the synchronization profile would be higher than the maximum acceleration of the slave axis.
                    Check when GearInSync_CheckMask_MaxAcc is active."},
            {17289, @"'The maximum deceleration of the synchronization profile would be higher than the maximum deceleration of the slave axis.
                    Check when GearInSync_CheckMask_MaxDec is active."},
            {17290, @"'The maximum jerk of the synchronization profile would be higher than the maximum jerk of the slave axis.
                    Check when GearInSync_CheckMask_MaxJerk is active(check the SlaveJerkMax)."},
            {17291, @"'The maximum jerk of the synchronization profile would be higher than the maximum jerk of the slave axis.
                    Check when GearInSync_CheckMask_MaxJerk is active(check the SlaveJerkMin)."},
            {17312, @"'Axis consequential error' Consequential error resulting from another causative error related to another axis. Axis consequential errors can occur in relation to master/slave couplings or with multiple axis interpolating DXD groups."},
            {17408, @"'Encoder ID not allowed' The value for the encoder ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is bigger than 255."},
            {17409, @"'Encoder type not allowed' The value for the encoder type is unacceptable because it is not defined. Type 1: Simulation (incremental) Type 2: M3000 (24 bit absolute) Type 3: M31x0 (24 bit incremental) Type 4: KL5101 (16 bit incremental) Type 5: KL5001 (24 bit absolute SSI) Type 6: KL5051 (16 bit BISSI)"},
            {17410, @"'Encoder mode' The value for the encoder (operating) mode is not allowed. Mode 1: Determination of the actual position Mode 2: Determination of the actual position and the actual velocity (filter)"},
            {17411, @"'Encoder counting direction inverted?' The flag for the encoder counting direction is not allowed. Flag 0: Positive encoder counting direction Flag 1: Negative encoder counting direction"},
            {17412, @"'Referencing status' The flag for the referencing status is not allowed. Flag 0: Axis has not been referenced Flag 1: Axis has been referenced"},
            {17413, @"'Encoder increments for each physical encoder rotation' The value for the number of encoder increments for each physical rotation of the encoder is not allowed. This value is used by the software for the calculation of encoder overruns and underruns."},
            {17414, @"'Scaling factor' The value for the scaling factor is not allowed. This scaling factor provides the weighting for the conversion of an encoder increment (INC) to a physical unit such as millimeters or degrees."},
            {17415, @"'Position offset (zero point offset)' The value for the position offset of the encoder is not allowed. This value is added to the calculated encoder position, and is interpreted in the physical units of the encoder."},
            {17416, @"'Modulo factor' The value for the encoder's modulo factor is not allowed."},
            {17417, @"'Position filter time' The value for the actual position filter time is not allowed (P-T1 filter)."},
            {17418, @"'Velocity filter time' The value for the actual velocity filter time is not allowed (P-T1 filter)."},
            {17419, @"'Acceleration filter time' The value for the actual acceleration filter time is not allowed (P-T1 filter)."},
            {17420, @"'Cycle time not allowed' (INTERNAL ERROR) The value of the SAF cycle time for the calculation of actual values is not allowed (e.g. is less than or equal to zero)."},
            {17421, @"'Configuration of the selected units is invalid' E.g. settings for modulo position, velocity per minute etc. lead to an error."},
            {17422, @"'Actual position correction / measurement system error correction' The value for the activation of the actual position correction ('measuring system error correction') is not allowed."},
            {17423, @"'Filter time actual position correction' The value for the actual position correction filter time is not allowed (P-T1 filter)."},
            {17424, @"'Search direction for referencing cam inverted' The value of the search direction of the referencing cam in a referencing procedure is not allowed. Value 0: Positive direction Value 1: Negative direction"},
            {17425, @"'Search direction for sync pulse (zero pulse) inverted' The value of the search direction of the sync pulse (zero pulse) in a referencing procedure is not allowed. Value 0: Positive direction Value 1: Negative direction"},
            {17426, @"'Reference position' The value of the reference position in a referencing procedure is not allowed."},
            {17427, @"'Clearance monitoring between activation of the hardware latch and appearance of the sync pulse' (NOT IMPLEMENTED) The flag for the clearance monitoring between activation of the hardware latch and occurrence of the sync/zero pulse ('latch valid') is not allowed. Value 0: Passive Value 1: Active"},
            {17428, @"'Minimum clearance between activation of the hardware latch and appearance of the sync pulse' (NOT IMPLEMENTED) The value for the minimum clearance in increments between activation of the hardware latch and occurrence of the sync/zero pulse ('latch valid') during a referencing procedure is not allowed."},
            {17429, @"'External sync pulse' (NOT IMPLEMENTED) The value of the activation or deactivation of the external sync pulse in a referencing procedure is not allowed. Value 0: Passive Value 1: Active"},
            {17430, @"'Scaling of the noise rate is not allowed' The value of the scaling (weighting) of the synthetic noise rate is not allowed. This parameter exists only in the simulation encoder and serves to produce a realistic simulation."},
            {17431, @"'Tolerance window for modulo-start' The value for the tolerance window for the modulo-axis-start is invalid. The value must be greater or equal than zero and smaller than the half encoder modulo-period (e. g. in the interval [0.0,180.0) )."},
            {17432, @"'Encoder reference mode' The value for the encoder reference mode is not allowed, resp. is not supported for this encoder type."},
            {17433, @"'Encoder evaluation direction' The value for the encoder evaluation direction (log. counter direction) is not allowed."},
            {17434, @"'Encoder reference system' The value for the encoder reference system is invalid (0: incremental, 1: absolute, 2: absolute+modulo)."},
            {17435, @"'Encoder position initialization mode' When starting the TC system the value for the encoder position initialization mode is invalid."},
            {17436, @"'Encoder sign interpretation (UNSIGNED- / SIGNED- data type)' The value for the encoder sign interpretation (data type) for the encoder the actual increment calculation (0: Default/not defined, 1: UNSIGNED, 2:/ SIGNED) is invalid."},
            {17437, @"'Homing Sensor Source' The value for the encoder homing sensor source is not allowed, resp. is not supported for this encoder type."},
            {17440, @"'Software end location monitoring minimum not allowed' The value for the activation of the software location monitoring minimum is not allowed."},
            {17441, @"'Software end location monitoring maximum not allowed' The value for the activation of the software location monitoring maximum is not allowed."},
            {17442, @"'Actual value setting is outside the value range' The 'set actual value' function cannot be carried out, because the new actual position is outside the expected range of values."},
            {17443, @"'Software end location minimum not allowed' The value for the software end location minimum is not allowed."},
            {17444, @"'Software end location maximum not allowed' The value for the software end location maximum is not allowed."},
            {17445, @"'Filter mask for the raw data of the encoder is invalid' The value for the filter mask of the encoder raw data in increments is invalid."},
            {17446, @"'Reference mask for the raw data of the encoder is invalid' The value for the reference mask (increments per encoder turn, absolute resolution) for the raw data of the encoder is invalid. E.g. this value is used for axis reference sequence (calibration) with the reference mode 'Software Sync'."},
            {17447, @"'Parameter Dead Time Compensation Mode (Encoder) is Invalid	
                  The parameter for the mode of dead time compensation at the NC encoder is invalid(OFF, ON with velocity, ON with velocity and acceleration).	"},
            {17448, @"'Parameter 'Control Bits of Dead Time Compensation' (Encoder) is Invalid	
                    The parameter for the control bits of dead time compensation at the encoder is invalid(e.g.relative or absolute time interpretation).	"},
            {17449, @"'Parameter 'Time Related Shift of Dead Time Compensation Mode' (Encoder) is Invalid	
                    The parameter for time related shift of dead time compensation(time shift in nanoseconds) at the encoder is invalid.   "},
            {17456, @"'Hardware latch activation (encoder)' Activation of the encoder hardware latch was implicitly initiated by the referencing procedure. If this function has already been activated but a latch value has not yet become valid ('latch valid'), another call to the function is refused with this error."},
            {17457, @"'External hardware latch activation (encoder)' The activation of the external hardware latch (only available on the KL5101) is initiated explicitly by an ADS command (called from the PLC program of the Visual Basic interface). If this function has already been activated, but the latch value has not yet been made valid by an external signal ('external latch valid'), another call to the function is refused with this error."},
            {17458, @"'External hardware latch activation (encoder)' If a referencing procedure has previously been initiated and the hardware still signals a valid latch value ('latch valid'), this function must not be called. In practice, however, this error can almost never occur."},
            {17459, @"'External hardware latch activation (encoder)' If this function has already been initiated and the hardware is still signaling that the external latch value is still valid ('extern latch valid'), a further activation should not be carried out and the commando will be declined with an error (the internal handshake communication between NC and IO device is still active). In that case the validity of the external hardware latch would immediately be signaled, although the old latch value would still be present."},
            {17460, @"'Encoder function not supported' An encoder function has been activated that is currently not released for use, or which is not even implemented."},
            {17461, @"'Encoder function is already active' An encoder function can not been activated because this functionality is already active."},
            {17472, @"'Encoder initialization' Encoder has not been initialized. Although the axis has been created, the rest of the initialization has not been performed (1. Initialization of axis I/O, 2. Initialization of axis, 3. Reset axis)."},
            {17473, @"'Axis address' The encoder does not have an axis, or the axis address has not been initialized."},
            {17474, @"'I/O input structure address' The drive does not have a valid I/O input address in the process image."},
            {17475, @"'I/O output structure address' The encoder does not have a valid I/O output address in the process image."},
            {17488, @"'Encoder counter underflow monitoring' The encoder's incremental counter has underflowed."},
            {17489, @"'Encoder counter overflow monitoring' The encoder's incremental counter has overflowed."},
            {17504, @"'Minimum Software Position Limit (Axis Start)'	
                    While monitoring of the minimum software position limit is active, an axis start has been performed towards a position that lies below the minimum software position limit. "},
            {17505, @"'Maximum Software Position Limit (Axis Start)'	
                    While monitoring of the maximum software position limit is active, an axis start has been performed towards a position that lies above the maximum software position limit. "},
            {17506, @"'Minimum Software Position Limit (Positioning Process)'	
                    While monitoring of the minimum software position limit is active, the actual position has fallen below the minimum software position limit.In case of servo axes, which are moved continuously, this limit is expanded by the magnitude of the parameterized following error position window. "},
            {17507, @"'Maximum Software Position Limit (Positioning Process)'	
                    While monitoring of the maximum software position limit is active, the actual position has exceeded the maximum software position limit. In case of servo axes, which are moved continuously, this limit is expanded by the magnitude of the parameterized following error position window. "},
            {17508, @"'Encoder hardware error' The drive resp. the encoder system reports a hardware error of the encoder. An optimal error code is displayed in the message of the event log."},
            {17509, @"'Position initialization error at system start' At the first initialization of the set position was this for all initialization trials (without over-/under-flow, with underflow and overflow ) out of the final position minimum and maximum."},
            {17510, @"'Invalid IO data for more than n subsequent NC cycles (encoder)	
                    The axis (encoder) has detected for more than n subsequent NC cycles (NC SAF task) invalid encoder IO data (e.g. n=3). Typically, regarding an EtherCAT member it is about a Working Counter Error (WcState) what displays that data transfer between IO device and controller is disturbed.	
                    If this error is set for a longer period of time continuously, this situation can lead to losing the axis reference(the 'homed' flag will be reset and the encoder will get the state 'unreferenced').
                    Possible reasons for this error: An EtherCAT slave may have left its OP state or there is a too high real time usage or a too high real time jitter.    "},
            {17511, @"'Invalid Actual Position (Encoder)	
                    The IO device delivers an invalid actual position(for CANopen / CoE look at bit 13 of encoder state 'TxPDO data invalid' or 'invalid actual position value').    "},
            {17512, @"'Invalid IO Input Data (Error Type 1)	
                    The monitoring of the 'cyclic IO input counter'(2 bit counter) has detected an error. The input data has not been refreshed for at least 3 NC SAF cycles(the 2 bit counter displays a constant value for multiple NC SAF cycles, instead of incrementing by exactly one from cycle to cycle). "},
            {17513, @"'Invalid IO Input Data (Error Type 2)	
                    The monitoring of the 'cyclic IO input counter'(2 bit counter) has detected an error. The quality of input data based on this two bit counter is not sufficient(there is here a simple statistic evaluation that evaluates GOOD cases and BAD cases and in exceeding a special limit value leads to an error).	"},
            {17520, @"'SSI transformation fault or not finished' The SSI transformation of the FOX 50 module was faulty for some NC-cycles or did not finished respectively."},
            {17570, @"'ENCERR_ADDR_CONTROLLER'"},
            {17571, @"'ENCERR_INVALID_CONTROLLERTYPE'"},
            {17664, @"'Controller ID not allowed' The value for the controller ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is greater than 255."},
            {17665, @"'Controller type not allowed' The value for the controller type is unacceptable because it is not defined. Type 1: P-controller (position) . . . Type 7: High/low speed controller Type 8: Stepper motor controller Type 9: Sercos controller"},
            {17666, @"'Controller operating mode not allowed' The value for the controller operating mode is not allowed."},
            {17667, @"'Weighting of the velocity pre-control not allowed' The value for the percentage weighting of the velocity pre-control is not allowed. The parameter is pre-set to 1.0 (100%) as standard."},
            {17668, @"'Following error monitoring (position) not allowed' The value for the activation of the following error monitoring is not allowed."},
            {17669, @"'Following error (velocity) not allowed' The value for the activation of the following error monitoring (velocity) is not allowed."},
            {17670, @"'Following error window (position) not allowed' The value for the following error window (maximum allowable following error) is not allowed."},
            {17671, @"'Following error filter time (position) not allowed' The value for the following error filter time (position) is not allowed."},
            {17672, @"'Following error window (velocity) not allowed' The value for the following error window (velocity) is not allowed."},
            {17673, @"'Following error filter time (velocity) not allowed' The value for the following error filter time (velocity) is not allowed."},
            {17674, @"'Controller Output Limitation' Improper	
                    The value for output limitation of the controller at the overall setpoint quantity is improper.The presetting amounts to 0.5(50 percent).Typically, this parameter is at work if to the motion controller device the velocity interface has been parameterized and the NC performs position control of the position on the controller.   "},
            {17680, @"'Proportional gain Kv or Kp (controller) not allowed' position The value for the proportional gain (Kv factor or Kp factor) is not allowed."},
            {17681, @"'Integral-action time Tn (controller) not allowed' position The value for the integral-action time is not allowed (I proportion of the PID T1 controller)."},
            {17682, @"'Derivative action time Tv (controller) not allowed' position The value for the derivative action time is not allowed (D proportion of the PID T1 controller)."},
            {17683, @"'Damping time Td (controller) not allowed' position The value for the damping time is not allowed (D proportion of the PID T1 controller). Suggested value: 0.1 * Tv"},
            {17684, @"'Activation of the automatic offset compensation not allowed' Activation of the automatic offset compensation is only possible for certain types of controller (with no I component)."},
            {17685, @"'Additional proportional gain Kv or Kp (controller) not allowed' position The value for the second term of the proportional gain (Kv factor or Kp factor) is not allowed."},
            {17686, @"'Reference velocity for additional proportional gain Kv or Kp (controller) not allowed' position The value for the reference velocity percentage data entry, to which the additional proportional gain is applied, is not allowed. The standard setting for the parameter is 0.5 (50%)."},
            {17687, @"'Proportional gain Pa (proportion) not allowed' acceleration The value for the proportional gain (Pa factor) is not allowed."},
            {17688, @"'Proportional gain Kv (velocity controller) not allowed' The value for the proportional gain (Kv factor) is not allowed."},
            {17689, @"'Reset time Tn (velocity controller) not allowed' The value for the integral-action time is not allowed (I proportion of the PID T1 controller)."},
            {17690, @"'Reserved	
                    Reserved, currently not used.   "},
            {17691, @"'Reserved	
                    Reserved, currently not used.   "},
            {17692, @"'Velocity Filter Time' Improper	
                    The parameter for velocity filter time in seconds is improper (P-T1 filter). This filter can be used within the NC for filtering an actual velocity or a velocity difference(velocity error = setpoint velocity - actual velocity) in special NC controllers(e.g.within the torque interface).	"},
            {17693, @"'Dead zone not allowed' The value for the dead zone from the position error or the velocity error (system deviation) is not allowed (only for complex controller with velocity or torque interface)."},
            {17695, @"'Proportionality Factor Kcp' Improper	
                    The parameter for the 'proportional factor Kcp' of the slave coupling differential control is improper. "},
            {17696, @"'Rate time Tv (velocity controller) not allowed' The value for the derivative action time is not allowed (D proportion of the PID T1 controller)."},
            {17697, @"'Damping time Td (velocity controller) not allowed' The value for the damping time is not allowed (D proportion of the PID T1 controller). Suggested value: 0.1 * Tv"},
            {17698, @"'Limitation of the I Part' Improper	
                    The parameter for limiting the I part of a PI or PID controller is improper.This inner state quantity can be limited in percent(1.0 refers to 100 percent).	"},
            {17699, @"'Limitation of the D Part' Improper	
                    The parameter for limitation of the D part of a PI or PID controller is improper.This inner state quantity may be limited in percent(1.0 refers to 100 percent).	"},
            {17700, @"'Parameter 'Switching Off the I Part During Motion' is Improper	
                    The boolean parameter for switching off the I part during an active positioning is improper.    "},
            {17701, @"'Parameter 'Filter Time for P-T2 Filter' Improper	
                    The time T0 in seconds is as filter time for the velocity controller P-T2 element improper.
                    The filter time has to be smaller than twice the NC-SAF cycle time. "},
            {17702, @"'Velocity Observer: 'Parameterized Mode' is Improper	
                    The parameterized mode(0=OFF, 1=LUENBERGER) for the special NC controller velocity observer within the torque interface is improper.   "},
            {17703, @"'Velocity Observer: 'Motor Torque Constant Kt or Kf' is Improper	
                    The parameter for the motor torque constant Kt(rotational motor) or Kf(linear motor) of the special NC controller velocity observer within the torque interface is improper.  "},
            {17704, @"'Velocity Observer: 'Motor Moment of Inertia JM' is Improper	
                    The parameter for the motor moment of inertia JM of the special NC controller velocity observer within the torque interface is improper.    "},
            {17705, @"'Velocity Observer: 'Band Width f0' is Improper	
                    The parameter for the band width f0 of the special NC controller velocity observer within the torque interface is improper.The band width has to be smaller than the reciprocal value of six times the NC cycle time(f0< 1/(6*T)).	"},
            {17706, @"'Velocity Observer: 'Correction Factor kc' is Improper	
                    The parameter for the correction factor kc of the special NC controller velocity observer within the torque interface is improper.The correction factor kc implements the relation between current and acceleration or angular acceleration.   "},
            {17707, @"'Velocity Observer: 'Time Constant T for First Order Filter' is Improper	
                    The time constant T for the first order velocity filter (PID-T2 or 'Lead Lag') of the specific NC controller velocity observer within the torque interface is improper.The correction factor kc implements the relation between current and acceleration or angular acceleration.  "},
            {17708, @"'Velocity Observer: 'Amplitude Damping d for Second Order Filter' is Improper	
                    The high pass/ low pass amplitude damping dHP or dTP for the second order velocity filter ('Bi-Quad') of the special NC controller velocity observer within the torque interface is improper.   "},
            {17709, @"'Velocity Observer: 'Frequency fHP or Frequency fTP for Filters of Second Order' is Improper	
                    The high pass frequency fHP or the low pass frequency fTP for the second order velocity filter('Bi-Quad') of the specific NC controller velocity observer within the torque interface is improper. "},
            {17728, @"'Controller initialization' Controller has not been initialized. Although the controller has been created, the rest of the initialization has not been performed (1. Initialization of controller, 2. Reset controller)."},
            {17729, @"'Axis address' Controller does not know its axis, or the axis address has not been initialized."},
            {17730, @"'Drive address' Controller does not know its drive, or the drive address has not been initialized."},
            {17744, @"'Following error monitoring (position)' With active following error monitoring (position) a following error exceedance has occurred, whose magnitude is greater than the following error window, and whose duration is longer than the parameterized following error filter time."},
            {17745, @"'Following error monitoring (velocity)' With active following error monitoring (velocity) a velocity following error exceedance has occurred, whose magnitude is greater than the following error window, and whose duration is longer than the parameterized following error filter time."},
            {17824, @"'CONTROLERR_RANGE_AREA_ASIDE'"},
            {17825, @"'CONTROLERR_RANGE_AREA_BSIDE'"},
            {17826, @"'CONTROLERR_RANGE_QNENN'"},
            {17827, @"'CONTROLERR_RANGE_PNENN'"},
            {17828, @"'CONTROLERR_RANGE_AXISIDPRESP0'"},
            {17920, @"'‘'Drive ID not allowed' The value for the drive ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is greater than 255."},
            {17921, @"'‘Drive type impermissible’ The value for the drive type is impermissible, since it is not defined."},
            {17922, @"'‘Drive operating mode impermissible’ The value for the drive operating mode is impermissible (mode 1: standard)."},
            {17923, @"'Motor polarity inverted?' The flag for the motor polarity is not allowed. Flag 0: Positive motor polarity flag 1: Negative motor polarity"},
            {17924, @"'‘Drift compensation/speed offset (DAC offset)’ The value for the drift compensation (DAC offset) is impermissible."},
            {17925, @"'‘Reference speed (velocity pre-control)’ The value for the reference speed (also called velocity pilot control) is impermissible."},
            {17926, @"'‘Reference output in percent’ The value for the reference output in percent is impermissible. The value 1.0 (100 %) usually corresponds to a voltage of 10.0 V."},
            {17927, @"'‘Quadrant compensation factor’ The value for the quadrant compensation factor is impermissible."},
            {17928, @"'‘Velocity reference point’ The value for the velocity reference point in percent is impermissible. The value 1.0 corresponds to 100 percent."},
            {17929, @"'‘Output reference point’ The value for the output reference point in percent is impermissible. The value 1.0 corresponds to 100 percent."},
            {17930, @"'‘Minimum or maximum output limits (output limitation)’ The value for the minimum and/or maximum output limit is impermissible. This will happen if the range of values is exceeded, the maximum limit is smaller than the minimum limit, or the distance between the minimum and maximum limits is zero. The minimum limit is initially set to –1.0 (-100 percent) and the maximum limit to 1.0 (100 percent)."},
            {17931, @"'Parameter 'Maximum Value for Output' is Improper	
                    The value for the maximum number of output digits of motion controllers(maximum output value) is improper.According to the used interface (e.g.position, velocity or torque/current). Regarding a velocity interface it is often about a signed 16 bit output value(± 32767).	"},
            {17932, @"'Parameter 'Internal Drive Control Word' is Improper	
                    The value as Internal Drive Control Word for the NC is improper.In this control word information from the system manager to the NC is contained what is evaluated at the TC start of the NC.	"},
            {17933, @"'Parameter 'Internal Timer for RESET Behavior of Motion Controller' is Improper	
                    The special parameter that influences the internal timing behavior between the NC motion controller and the IO motion controller is improper.   "},
            {17934, @"'Parameter 'Master Motion Controller ID' is Improper	
                    The parameter 'master motion controller ID' is improper for a further NC motion controller in slave mode.An additional NC motion controller in slave mode can be used if this usage is about the same motion controller device on that different NC information for e.g.different operation modes are joined (e.g.velocity mode and torque mode).	
                    Note: This parameter is not accessible by the user directly, but can be influenced indirectly by the configuration of additional NC motion controllers below the NC axis.   "},
            {17935, @"'‘Drive torque output scaling impermissible’ The value is impermissible as drive torque output scaling (rotary motor) or as force output scaling (linear motor)."},
            {17936, @"'Drive velocity output scaling is not allowed' The value for the drive velocity output scaling is not allowed."},
            {17937, @"'‘Profi Drive DSC proportional gain Kpc (controller) impermissible’ Positions The value for the Profi Drive DSC position control gain (Kpc factor) is impermissible."},
            {17938, @"'‘Table ID is impermissible’ The value for the table ID is impermissible."},
            {17939, @"'‘Table interpolation type is impermissible’ The value is impermissible as the table interpolation type."},
            {17940, @"'‘Output offset in percent is impermissible’ The value is impermissible as an output offset in percent (+/- 1.0)."},
            {17941, @"'‘Profi Drive DSC scaling for calculation of 'Xerr' (controller) impermissible’ Positions: the value is impermissible as Profi Drive DSC scaling for the calculation of ‘Xerr’."},
            {17942, @"'‘Drive acceleration output scaling impermissible’ The value is impermissible as drive acceleration/deceleration output scaling."},
            {17943, @"'‘Drive position output scaling impermissible’ The value is impermissible as drive position output scaling."},
            {17944, @"'Parameter 'Dead Time Compensation Mode' (Motion Controller) is Invalid	
                    The parameter for the mode of dead time compensation of NC motion controllers is invalid (OFF, ON with velocity, ON with velocity and acceleration).	"},
            {17945, @"'Parameter 'Control Bits of Dead Time Compensation' (Motion Controller) is Invalid	
                    The parameter for 'control bits of dead time compensation' of NC motion controllers is invalid(e.g.relative or absolute time interpretation).	"},
            {17946, @"'Parameter 'Time Shift of Dead Time Compensation' (Motion Controller) is Invalid	
                    The parameter for the time shift of dead time compensation(time shift in nanoseconds) of the NC motion controller is invalid.  "},
            {17947, @"'Parameter 'Output Delay (Velocity)' is Invalid	
                    The parameter for an optional output delay within the velocity interface to the motion controller is invalid(output delay (velocity)). The maximum permitted delay time has to be smaller than 100 times the NC SAF cycle time.    "},
            {17948, @"'‘Drive filter type impermissible for command variable filter for the output position’ The value is impermissible as a drive filter type for the smoothing of the output position (command variable filter for the setpoint position)."},
            {17949, @"'‘Drive filter time impermissible for command variable filter for the output position’ The value is impermissible as a drive filter time for the smoothing of the output position (command variable filter for the setpoint position)."},
            {17950, @"'‘Drive filter order impermissible for command variable filter for the output position’ The value is impermissible as a drive filter order (P-Tn) for the smoothing of the output position (command variable filter for the setpoint position)."},
            {17952, @"'‘Bit mask for stepper motor cycle impermissible’ A value of the different stepper motor masks is impermissible for the respective cycle."},
            {17953, @"'‘Bit mask for stepper motor holding current impermissible’ The value for the stepper motor holding mask is impermissible."},
            {17954, @"'‘Scaling factor for actual torque (actual current) impermissible’ The value is impermissible as a scaling factor for the actual torque (or actual current)."},
            {17955, @"'‘Filter time for actual torque is impermissible’ The value is impermissible as a filter time for the actual torque (or the actual current) (P-T1 filter)."},
            {17956, @"'‘Filter time for the temporal derivation of the actual torque is impermissible’ The value is impermissible as a filter time for the temporal derivation of the actual torque (or actual current (P-T1 filter)."},
            {17957, @"'Parameter for the 'Motion Controller Operation Mode' is Invalid	
                    The parameter for the motion controller operation mode (position mode, velocity mode, torque mode, …) is invalid.Possibly, an NC operation mode switching has been tried or at TC system start has been tried to activate a preconfigured operation mode.
                    Annotations: The generic operation modes defined within the NC are realized by the NC motion controller specifically, i.e. in particular for the protocols SERCOS/ SoE and CANopen/ CoE (DS402). In this connection, protocol specific, motion controller specific or even customer specific peculiarities have to be obeyed (e.g.regarding SERCOS/ SoE merely in the SERCOS parameter range S-0-0032 to S-0-0035 predefined operation modes can be activated at runtime). Furthermore, not every generic NC operation mode can be converted into a motion controller specific operation mode(here gaps within the specification may exist).	
                    The generic NC operation mode 0 forms a special case. This value is used as a mark to activate an NC default operation mode(as long as this mark is known to the NC).	"},
            {17958, @"'Motion Controller Functionality is Not Supported	
                    A motion controller functionality has been set off that has not been released for usage or has not been implemented(e.g.writing or reading of a motion controller mode that is not supported by certain motion controllers). It is also possible that this functionality is merely not supported at times (e.g.because the motion controller device resides in error state or a motion controller enable is missing).	"},
            {17959, @"'DRIVEOPERATIONMODEBUSY. The activation of the motion controller controlling mode has failed because another object with OID… uses this interface already."},
            {17960, @"'Motion Controller Operation Mode Switching has not been configured or the desired motion controller operation mode cannot be found	
                    There has not any motion controller operation mode switching been configured and thus no reading or writing of a motion controller operation mode is possible. Or the desired motion control operation mode has not been found in the list of the predefined motion controller operation modes (e.g. for SoE/ SERCOS).	
                    Annotation for CoE motion controllers: The reading or writing of the CoE Motion Control Operation Mode is merely possible if the CoE objects 0x6060 Modes Of Operation and 0x6061 Modes Of Operation Display can be found in the cyclic process data (PDO list) and a valid default operation mode has been configured.	
                    Annotation for SoE motion controllers: The reading or writing of the current SoE Motion Controller Operation Mode is merely possible if this operation mode has been predefined in one of the SoE Parameters S-0-0032 to S-0-0035.	"},
            {17968, @"'‘Overtemperature’ Overtemperature was detected or reported in the drive or terminal."},
            {17969, @"'‘Undervoltage’ Undervoltage was detected or reported in the drive or terminal."},
            {17970, @"'‘Wire break in phase A’ A wire break in phase A was detected or reported in the drive or terminal."},
            {17971, @"'‘Wire break in phase B’ A wire break in phase B was detected or reported in the drive or terminal."},
            {17972, @"'‘Overcurrent in phase A’ Overcurrent was detected or reported in phase A in the drive or terminal."},
            {17973, @"'‘Overcurrent in phase B’ Overcurrent was detected or reported in phase B in the drive or terminal."},
            {17974, @"'‘Torque overload (stall)’ A torque overload (stall) was detected or reported in the drive or terminal."},
            {17984, @"'‘Drive initialization’ Drive has not been initialized. Although the drive has been created, the rest of the initialization has not been performed (1. Initialization of drive I/O, 2. Initialization of drive, 3. Reset drive)."},
            {17985, @"'‘Axis address’ Drive does not know its axis, or the axis address has not been initialized."},
            {17986, @"'‘Address IO input structure’ Drive has no valid IO input address in the process image."},
            {17987, @"'‘Address IO output structure’ Drive has no valid IO output address in the process image."},
            {18000, @"'‘Drive hardware not ready to operate’ The drive hardware is not ready for operation. The following are possible causes:	
                    - the drive is in the error state (hardware error)	
                    - the drive is in the start-up phase (e.g. after an axis reset that was preceded by a hardware error)	
                    - the drive is missing the controller enable (ENABLE)	
                    Note: The time required for 'booting' a drive after a hardware fault can amount to several seconds.	"},
            {18001, @"'Error in the cyclic communication of the drive (Life Counter). Reasons for this could be an interrupted fieldbus or a drive that is in the error state."},
            {18002, @"'‘Changing the table ID when active controller enable is impermissible’. Changing (deselecting, selecting) the characteristic curve table ID is not permissible when the controller enable for the axis is active."},
            {18005, @"'‘Invalid IO data for more than ‘n’ continuous NC cycles’ The axis (encoder or drive) has detected invalid IO data (e.g. n=3) for more than ‘n’ continuous NC cycles (NC SAF task).	
                    EtherCAT fieldbus: ‘working counter error ('WCState')’As a result it is possible that the encoder referencing flag will be reset to FALSE (i.e. the encoder is given the status ‘unreferenced’).	
                    Lightbus fieldbus: ‘CDL state error ('CdlState')’	
                    As a result it is possible that the encoder calibration flag will set to FALSE(that means uncalibrated).	"},
            {18944, @"'Table ID not allowed' The value for the table ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is greater than 255."},
            {18945, @"'Table type not allowed' The value for the table type is unacceptable because it is not defined."},
            {18946, @"'Number of lines in the table not allowed' The value of the number of lines in the table is not allowed, because, for example, it is smaller than two at linear interpolation and smaller than four at spline interpolation."},
            {18947, @"'Number of columns in the table is not allowed' The value of the number of columns in the table is not allowed, because, for example, it is less than or equal to zero (depends upon the type of table or slave)."},
            {18948, @"'Step size (position delta) not allowed' The value for the step size between two lines (position delta) is not allowed, because, for example, it is less than or equal to zero."},
            {18949, @"'Period not allowed' The value for the period is not allowed, because, for example, it is less than or equal to zero."},
            {18950, @"'Table is not monotonic' The value for the step size is not allowed, because, for example, it is less than or equal to zero."},
            {18951, @"'Table sub type is not allowed' The value for the table sub type is not allowed or otherwise the table class (slave type) do not match up to the table main type. Table sub type: (1) equidistant linear position table, (2) equidistant cyclic position table, (3) none equidistant linear position table, (4) none equidistant cyclic position table"},
            {18952, @"'Table interpolation type is not allowed' The value for the table interpolation type is allowed. Table interpolation type: (0) linear-interpolation, (1) 4-point-interpolation, (2) spline-interpolation"},
            {18953, @"'Incorrect table main type' The table main type is unknown or otherwise the table class (slave type) do not match up to the table main type. Table main type: (1) camming table, (2) characteristic table, (3) 'motion function' table (MF)"},
            {18960, @"'Table initialization' Table has not been initialized. Although the table has been created, the rest of the initialization has not been performed. For instance, the number of lines or columns may be less than or equal to zero."},
            {18961, @"'Not enough memory' Table could not be created, since there is not enough memory."},
            {18962, @"'Function not executed, function not available' The function has not been implemented, or cannot be executed, for the present type of table."},
            {18963, @"'Line index not allowed' The start line index or the stop line index to be used for read or write access to the table is not allowed. For instance, the line index may be greater than the total number of lines in the table."},
            {18964, @"'Column index not allowed' The start column index or the stop column index to be used for read or write access to the table in not allowed. For instance, the column index may be greater than the total number of columns in the table."},
            {18965, @"'Number of lines not allowed' The number of lines to be read from or written to the table is not allowed. The number of lines must be an integer multiple of the number of elements in a line (n * number of columns)."},
            {18966, @"'Number of columns not allowed' The number of columns to be read from or written to the table is not allowed. The number of columns must be an integer multiple of the number of elements in a column (n * number of lines)."},
            {18967, @"'Error in scaling or in range entry' The entries in the table header are inconsistent, e.g. the validity range is empty. If the error is generated during the run time it is a run time error and stops the master/slave group."},
            {18968, @"'Multi table slave out of range' The slave master position is outside the table values for the master. The error is a run-time error, and stops the master/slave group."},
            {18969, @"'Solo table underflow' The slave master position is outside the table values for the master. The master value of the equidistant table, to be processed linearly, lies under the first table value. The error is a run-time error, and stops the master/slave group."},
            {18970, @"'Solo table overflow' The slave master position is outside the table values for the master. The master value of the equidistant table, to be processed linearly, lies above the first table value. The error is a run-time error, and stops the master/slave group."},
            {18971, @"'Incorrect execution mode' The cyclic execution mode can only be 'true' or 'false'."},
            {18972, @"'Impermissible parameter' The Fifo parameter is not allowed."},
            {18973, @"'Fifo is empty' The Fifo of the external generator is empty. This can signify end of track or a run time error."},
            {18974, @"'Fifo is full' The Fifo of the external generator is full. It is the user‘s task to continue to attempt to fill the Fifo with the rejected values."},
            {18975, @"'Point-Index of Motion Function invalid' The point index of a Motion Function Point of a Function Table is invalid. First the point index has to be larger than zero and second it has to be numerical continuously for one column in the Motion Function Table (e.g. 1,2,3,... or 10,11,12,...).
                    Remark: The point index is not online - changeable but must be constant."},
            {18976, @"'No diagonalization of matrix' The spline can not be calculated. The master positions are not correct."},
            {18977, @"'Number of spline points to less' The number of points of a cubic spline has to be greater than two."},
            {18978, @"'Fifo must not be overwritten' Fifo must not be overwritten since then the active line would be overwritten. It is the task of the user to secure that the active line is not modified."},
            {18979, @"'Insufficient number of Motion Function points' The number of valid Motion Function points is less than two. Either the entire number of points is to low or the point type of many points is set to Ignore Point."},
            {18981, @"'Table master start position is not allowed' A periodic position table must start with a master position zero. A Motion Function (MF) table can start at a position greater than zero but less than the cam period."},
            {19200, @"'Axis was stopped' The axis was stopped during travel to the target position. The axis may have been stopped with a PLC command via ADS, a call via AXFNC, or by the System Manager."},
            {19201, @"'Axis cannot be started' The axis cannot be started because:
                    the axis is in error status,
                    the axis is executing another command,
                    the axis is in protected mode,
                    the axis is not ready for operation."},
            {19202, @"'Control mode not permitted' No target position control, and no position range control."},
            {19203, @"'Axis is not moving' The position and velocity can only be restarted while the axis is physically in motion."},
            {19204, @"'Invalid mode'
                   Examples: Invalid Direction with MC_MoveModulo.Inactive axis parameter Position correction with MC_BacklashCompensation."},
            {19205, @"'Command not permitted'
                    Continuous motion in an unspecified direction
                    Read/Write parameters: type mismatch"},
            {19206, @"'Parameter incorrect'
                    Incorrect override: > 100% or< 0%
                    Incorrect gear ratio: RatioDenominator = 0"},
            {19207, @"'Timeout axis function block'
                    After positioning, all 'MC_Move...' blocks check whether positioning was completed successfully.In the simplest case, the 'AxisHasJob' flag of the NC axis is checked, which initially signifies that positioning was logically completed.Depending on the parameterization of the NC axis, further checks (quality criteria) are used:
                    'Position range monitoring'
                    If position range monitoring is active, the system waits for feedback from the NC.After positioning, the axis must be within the specified positioning range window. If necessary, the position controller ensures that the axis is moved to the target position.If the position controller is switched off (Kv= 0) or weak, the target may not be reached.
                    'Target position monitoring'
                    If target position monitoring is active, the system waits for feedback from the NC.After positioning, the axis must be within the specified target position window for at least the specified time.If necessary, the position controller ensures that the axis is moved to the target position.If the position controller is switched off (Kv= 0) or weak, the target may not be reached.Floating position control may lead to the axis oscillating around the window but not remaining inside the window.
                    If the axis is logically at the target position (logical standstill) but the parameterized position window has not been reached, monitoring of the above-mentioned NC feedback is aborted with error 19207 (0x4B07) after a constant timeout of 6 seconds."},
            {19208, @"'Axis is in protected mode' The axis is in protected mode (e.g., coupled) and cannot be moved."},
            {19209, @"'Axis is not ready' The axis is not ready and cannot be moved."},
            {19210, @"'Error during referencing' Referencing (homing) of the axis could not be started or was not successful."},
            {19211, @"'Incorrect definition of the trigger input' The definition of the trigger signal for function block MC_TouchProbe is incorrect. The defined encoder-ID, the trigger signal or the trigger edge are invalid."},
            {19212, @"'Position latch was disabled' The function block MC_TouchProbe has detected that a measuring probe cycle it had started was disabled. The reason may be an axis reset, for example."},
            {19213, @"'NC status feedback timeout' A function was successfully sent from the PLC to the NC. An expected feedback in the axis status word has not arrived."},
            {19214, @"'Additional product not installed' The function is available as an additional product but is not installed on the system."},
            {19215, @"'No NC Cycle Counter Update' – The NcToPlc Interface or the NC Cycle Counter in the NcToPlc Interface was not updated."},
            {19216, @"'M-function query missing' This error occurs if the M-function was confirmed, but the request bit was not set."},
            {19217, @"'Zero shift index is outside the range' The index of the zero shift is invalid."},
            {19218, @"'R-parameter index or size is invalid' This error occurs if the R-parameters are written or read but the index or size are outside the range."},
            {19219, @"'Index for tool description is invalid'"},
            {19220, @"'Version of the cyclic channel interface does not match the requested function or the function block' This error occurs if an older TwinCAT version is used to call new functions of a later TcNci.lib version."},
            {19221, @"'Channel is not ready for the requested function' The requested function cannot be executed, because the channel is in the wrong state. This error occurs during reverse travel, for example, if the axis was not stopped with ItpEStop first."},
            {19222, @"'Requested function is not activated' The requested function requires explicit activation."},
            {19223, @"'Axis is already in another group' The axis has already been added to another group."},
            {19224, @"'Block search could not be executed successfully' The block search has failed.
                    Possible causes:
                    Invalid block number"},
            {19225, @"'Invalid block search parameter' This error occurs if the FB ItpBlocksearch is called with invalid parameters (e.g., E_ItpDryRunMode, E_ItpBlockSearchMode)"},
            {19232, @"'Cannot add all axes' This error occurs if an auxiliary axis is to be added to an interpolation group, but the function fails. It is likely that a preceding instruction of an auxiliary axis was skipped."},
            {19248, @"'Pointer is invalid' A pointer to a data structure is invalid, e.g., Null
                    Data structure MC_CAM_REF was not initialized"},
            {19249, @"'Memory size invalid' The specification of the memory size (SIZE) for a data structure is invalid.
                    The value of the size parameter is 0 or less than the size of one element of the addressed data structure.
                    The value of the size parameter is less than the requested amount of data.
                    The value of the size parameter does not match other parameters as number of points, number of rows or number of columns."},
            {19250, @"'Cam table ID is invalid' The ID of a cam table is not between 1 and 255."},
            {19251, @"'Point ID is invalid' The ID of a point (sampling point) of a motion function is less than 1."},
            {19252, @"'Number of points is invalid' The number of points (sampling points) of a cam plate to be read or written is less than 1."},
            {19253, @"'MC table type is invalid' The type of a cam plate does not match the definition MC_TableType."},
            {19254, @"'Number of rows invalid' The number of rows (sampling points) of a cam table is less than 1."},
            {19255, @"'Number of columns invalid' The number of columns of a cam table is invalid.
                    The number of columns of a motion function is not equal 1
                    The number of columns of a standard cam table is not equal 2
                    The number of columns does not match another parameter(ValueSelectMask)"},
            {19256, @"'Step size invalid'. The increment for the interpolation is invalid, e.g., less than or equal to zero."},
            {19264, @"'Terminal type not supported' The terminal used is not supported by this function block."},
            {19265, @"'Register read/write error' This error implies a validity error."},
            {19266, @"'Axis is enabled' The axis is enabled but should not be enabled for this process."},
            {19267, @"'Incorrect size of the compensation table' The specified table size (in bytes) does not match the actual size"},
            {19268, @"'The minimum/maximum position in the compensation table does not match the position in the table description (ST_CompensationDesc)"},
            {19269, @"'Not implemented' The requested function is not implemented in this combination"},
            {19270, @"'Window not in the specified modulo range' The parameterized min or max position is not in the specified modulo range"},
            {19271, @"'Buffer overflow' The number of events has led to an overflow of the buffer and not all events could be acquired."},
            {19296, @"'Motion command did not become active' A motion command has been started and has been buffered and confirmed by the NC. Nevertheless, the motion command did not become active (possibly due to a terminating condition or an internal NC error)."},
            {19297, @"'Motion command could not be monitored by the PLC' A motion command has been started and has been buffered and confirmed by the NC. The PLC has not been able to monitor the execution of this command and the execution status is unclear since the NC is already executing a more recent command. The execution state is unclear. This error may come up with very short buffered motion commands which are executed during one PLC cycle."},
            {19298, @"'Buffered command was terminated with an error' A buffered command was terminated with an error. The error number is not available, because a new command is already being executed."},
            {19299, @"'Buffered command was completed without feedback' A buffered command was completed but there was no feedback to indicate success or failure."},
            {19300, @"' 'BufferMode' is not supported by the command' The 'BufferMode' is not supported by this command."},
            {19301, @"'Command number is zero' The command number for queued commands managed by the system unexpectedly has the value 0."},
            {19302, @"'Function block was not called cyclically' The function block was not called cyclically. The command execution could not be monitored by the PLC, because the NC was already executing a subsequent command. The execution state is unclear."},
            {19313, @"'Invalid NCI entry type'. The FB FB_NciFeedTablePreparation was called with an unknown nEntryType."},
            {19314, @"'NCI feed table full' The table is full, and the entry is therefore not accepted.
                    Remedy:
                    Transfer the context of the table with FB_NciFeedTable to the NC kernel.If bFeedingDone = TRUE, the table can be reset in FB_NciFeedTablePreparation with bResetTable and then filled with new entries."},
            {19315, @"'internal error"},
            {19316, @"'ST_NciTangentialFollowingDesc: Tangential axis is not an auxiliary axis' In the entry for the tangential following, a tangential axis was named that is not an auxiliary axis."},
            {19317, @"'ST_NciTangentialFollowingDesc: nPathAxis1 or nPathAxis2 is not a path axis. It is therefore not possible to determine the plane."},
            {19318, @"'ST_NciTangentialFollwoingDesc: nPathAxis1 and nPathAxis2 are the same. It is therefore not possible to determine the plane."},
            {19319, @"'ST_NciGeoCirclePlane: Circle incorrectly parameterized"},
            {19320, @"'Internal error during calculation of tangential following"},
            {19321, @"'Tangential following: Monitoring of the deviation angle was activated during activation of tangential following (E_TfErrorOnCritical1), and an excessively large deviation angle was detected in the current segment."},
            {19322, @"'not implemented"},
            {19323, @"'Tangential following: the radius of the current arc is too small"},
            {19324, @"'FB_NciFeedTablePreparation: pEntry is NULL"},
            {19325, @"'FB_NciFeedTablePreparation: the specified nEntryType does not match the structure type"},
            {19326, @"'ST_NciMFuncFast and ST_NciMFuncHsk: the requested M-function is not between 0 and 159"},
            {19327, @"'ST_NciDynOvr: the requested value for the dynamic override is not between 0.01 and 1"},
            {19328, @"'ST_NciVertexSmoothing: invalid parameter. This error is generated if a negative smoothing radius or an unknown smoothing type is encountered."},
            {19329, @"'FB_NciFeedTablePrepartion: The requested velocity is not in the valid range"},
            {19330, @"'ST_Nci*: invalid parameter"},
            {19344, @"'Determined drive type is not supported"},
            {19345, @"'Direction is impermissible"},
            {19346, @"'SwitchMode is impermissible"},
            {19347, @"'Mode for the parameter handling is impermissible"},
            {19348, @"'Parameterization of the torque limits is inconsistent"},
            {19349, @"'Parameterization of the position lag limit is impermissible (<=0)."},
            {19350, @"'Parameterization of the distance limit is impermissible (<0)"},
            {19351, @"'An attempt was made to back up parameters again, although they have already been backed up."},
            {19352, @"'An attempt was made to restore parameters, although none have been backed up."},
            {19359, @"'The abortion of a homing has failed."},
            {19360, @"'KinGroup error: the kinematic group is in an error state.
                    This error may occur if the kinematic group is in an error state or an unexpected state when it is called(e.g., simultaneous call via several FB instances)."},
            {19361, @"'KinGroup timeout: timeout during call of a kinematic block"},
            {19376, @"'The current axis position or the axis position resulting from the new position offset exceeds the valid range of values."},
            {19377, @"'The new position offset exceeds the valid range of values [AX5000: 2^31]."},
            {19378, @"'The current axis position or the axis position resulting from the new position offset falls below the valid range of values."},
            {19379, @"'The new position offset falls below the valid range of values [AX5000: -2^31]."},
            {19380, @"'The activated feedback and/or storage location (AX5000: P-0-0275) differ from the parameterization on the function block."},
            {19381, @"'Reinitialization of the actual NC position has failed, e.g., reference system = 'ABSOLUTE (with single overflow)' & software end position monitoring is disabled."},
            {19382, @"'The command to set or delete a position offset was rejected without feedback data, e.g., if the drive controller's firmware does not support the corresponding command."},
            {19383, @"'The command to set or delete a position offset was rejected with feedback data. The information in the feedback data may contain further information about the cause.
                    e.g., if the drive controller's firmware does not support the corresponding command."},
            {19384, @"'A firmware version >= 19 is required for the servo terminal."},
            {19385, @"'The modulo settings on the drive controller and NC are different."},
            {19394, @"'The new position offset exceeds the valid value range."},
            {19395, @"'I/O data are invalid or the terminal is in an error state."},
            {19456, @"'Transformation failed."},
            {19457, @"'Ambiguous answer. The answer of the transformation is not explicit."},
            {19458, @"'Invalid axis position: The transformation can not be calculated with the current position data.
                    Possible causes:
                    The position is outside the working area of the kinematics"},
            {19459, @"'Invalid dimension: The dimension of the paramerterized input parameter does not match the dimension expected by the kinematic object.
                    Possible causes:
                    Too many position values are supplied for this configuration.Check the number of parameterized axes."},
            {19460, @"'NCERR_KINTRAFO_REGISTRATION"},
            {19461, @"'Newton iteration failed: The Newton iteration does not converge."},
            {19462, @"'Jacobi matrix cannot be inverted"},
            {19463, @"'Invalid cascade: This kinematic configuration is not permitted."},
            {19464, @"'Singularity: The machine configuration results in singular axis velocities."},
            {19467, @"'No metainfo: Metainfo pointer is null."},
            {19475, @"'NCERR_RBTFRAME_INVALIDWCSTOMCS
                    The employed WcsToMcs component leads to positions that the selected kinematics cannot adopt to.
                    Tailoring the WcsToMcs parameters is required."},
            {19488, @"'Transformation failed: Call of extended kinematic model failed."},
            {19504, @"'Invalid input frame: Programmed Cartesian position cannot be reached in the ACS configuration."},
            {19536, @"'Invalid Offset: Access violation within the observer detected."},
            {33024, @"'Internal error"},
            {33025, @"'Not initialized (e.g. no nc axis)"},
            {33026, @"'Invalid parameter"},
            {33027, @"'Invalid index offset"},
            {33028, @"'Invalid parameter size"},
            {33029, @"'Invalid start parameter (set point generator)"},
            {33030, @"'Not supported"},
            {33031, @"'Nc axis not enabled"},
            {33032, @"'Nc axis in error state"},
            {33033, @"'IO drive in error state"},
            {33034, @"'Nc axis AND IO drive in error state"},
            {33035, @"'Invalid drive operation mode active or requested
                    (no bode plot mode)"},
            {33036, @"'Invalid context for this command (mandatory task or windows context needed)"},
            {33037, @"'Missing TCom axis interface (axis null pointer).
                    There is no connection to the NC axis.
                    Either no axis (or axis ID) has been parameterized, or the parameterized axis does not exist."},
            {33038, @"'Invalid input cycle counter from IO drive (e.g. frozen).
                    The cyclic drive data are backed up by an ‘InputCycleCounter’ during the bode plot recording. This allows firstly the detection of an unexpected communication loss (keyword: LifeCounter) and secondly a check for temporal data consistency to be performed.
                    Sample 1: This error can occur if the cycle time of the calling task is larger than the assumed drive cycle time (in this case, however, the error occurs right at the start of the recording).
                    Sample 2: This error can occur if the calling task has real-time errors (e.g. the 'Exceed Counter' of the task increments or the task has a lower priority, as is often the case, for example, with the PLC). In this case the error can also occur at any time during the recording.
                    Sample 3: This error can occur more frequently if the real-time load on the computer is quite high (>50 %).
                    Note: Refer also to the corresponding AX5000 drive error code F440."},
            {33039, @"'Position monitoring: Axis position is outside of the maximum allowed moving range.
                    The axis has left the parameterized position range window, whereupon the recording was aborted and the NC axis was placed in the error state 0x810F (with standard NC error handling).
                    The position range window acts symmetrically around the initial position of the axis (see also parameter description Position Monitoring Window).
                    Typical error message in the logger:
                    'BodePlot: 'Position Monitoring' error 0x%x because the actual position %f is above the maximum limit %f of the allowed position range (StartPos=%f, Window=%f)'"},
            {33040, @"'Driver limitations detected (current or velocity limitations) which causes a nonlinear behavior and invalid results of the bode plot.
                    A bode plot recording requires an approximately linear transmission link. If the speed or current is limited in the drive unit, however, this non-linear behavior is detected and the bode plot recording is aborted. Reasons for these limitations can be: choosing too large an amplitude for the position, speed or torque interface, or an unsuitable choice of amplitude scaling mode(see also parameter description Amplitude Scaling Mode, Base Amplitude, Signal Amplitude).
                    Typical error message in the logger:
                    'BodePlot: Sequence aborted with error 0x%x because the current limit of the drive has been exceeded (%d times) which causes a nonlinear behavior and invalid results of the bode plot'"},
            {33041, @"'Life counter monitoring (heartbeat): Lost of communication to GUI detected after watchdog timeout is elapsed.
                    The graphical user interface from which the bode plot recording was started is no longer communicating with the bode plot driver in the expected rhythm(keyword: ‘Life Counter’). Therefore the recording is terminated immediately and the NC axes are placed in the error state 0x8111 (with standard NC error handling). Possible reasons for this can be an operating interface crash or a major malfunction of the Windows context.
                    Typical error message in the logger:
                    'BodePlot: Sequence aborted with GUI Life Counter error 0x%x because the WatchDog timeout of %f s elapsed ('%s')'"},
            {33042, @"'WC state error (IO data working counter)
                    IO working counter error (WC state), for example due to real-time errors, EtherCAT CRC errors or telegram failures, EtherCAT device not communicating (OP state), etc."},
            {33043, @"'Reserved area"},
            {33044, @"'Reserved area"},
            {33045, @"'Reserved area"},
            {33046, @"'Reserved area"},
            {33047, @"'Reserved area"},
            {33048, @"'Reserved area"},
            {33049, @"'Reserved area"},
            {33050, @"'Reserved area"},
            {33051, @"'Reserved area"},
            {33052, @"'Reserved area"},
            {33053, @"'Reserved area"},
            {33054, @"'Reserved area"},
            {33055, @"'Reserved area"},
            {33056, @"'Invalid configuration for Object (e.g. in System Manager)."},
            {33057, @"'Invalid environment for Object (e.g. TcCom-Object's Hierarchy or missing/faulty Objects)."},
            {33058, @"'Incompatible Driver or Object."},
            {33060, @"'Command execution does not terminate (e. g. MC_Reset does not signal DONE)."},
            {33072, @"'Invalid ObjectID of Communication Target."},
            {33073, @"'Communication Target expects Call in different Context."},
            {33074, @"'Invalid State of Communication Target."},
            {33076, @"'Communication with Communication Target cannot be established."},
            {33083, @"'Transition Mode is invalid."},
            {33084, @"'BufferMode is invalid."},
            {33085, @"'Only one active Instance of Function Block per Group is allowed."},
            {33086, @"'Command is not allowed in current group state."},
            {33087, @"'Slave cannot synchronize. The slave cannot reach the SlaveSyncPosition with the given dynamics."},
            {33088, @"'Invalid value for one or more of the dynamic parameters (Acceleration, Deceleration, Jerk)."},
            {33089, @"'IdentInGroup is invalid."},
            {33090, @"'The number of axes in the group is incompatible with the axes convention."},
            {33091, @"'Function Block or respective Command is not supported by Target."},
            {33092, @"'Command queue full. Command queue is completely filled up and cannot accept additional commands until some commands are fully processed."},
            {33093, @"'Mapping of Cyclic Interface between NC and PLC is missing (e.g. AXIS_REF, AXES_GROUP_REF, ...)."},
            {33094, @"'Invalid Velocity Value. The velocity was not set or the entered value is invalid"},
            {33095, @"'Invalid Coordinate Dimension. The dimension of the set coordinate interpretation does not meet the requirements."},
            {33096, @"'Invalid Input Value."},
            {33097, @"'Unsupported Dynamics for selected Group Kernel."},
            {33098, @"'The programmed position dimension incompatible with the axes convention."},
            {33099, @"'Path buffer is invalid. E.g. because provided buffer has invalid address or is not big enough."},
            {33100, @"'Path does not contain any element."},
            {33101, @"'Provided Path buffer is too small to store more Path Elements."},
            {33102, @"'Dimension or at least one Value of Transition Parameters is invalid."},
            {33103, @"'Invalid or Incomplete Input Array."},
            {33104, @"'Path length is zero."},
            {33105, @"'Command is not allowed in current axis state."},
            {33106, @"'TwinCAT System is shutting down and cannot complete request."},
            {33107, @"'Configured axes convention and configured axes do not match."},
            {33108, @"'Invalid Number of ACS Axes. The number of ACS input axes does not match the number of ACS input axes expected by the kinematic transformation."},
            {33109, @"'Invalid Number of MCS Data. The number of MCS input data does not match the number expected by the kinematic transformation."},
            {33110, @"'Invalid Value Set for Kinematic Parameters. The numeric value set for the parameter does not reside within the respective definition range."},
            {33112, @"'The Given ACS Values Cannot be Reached. The given ACS values result in an invalid machine configuration."},
            {33113, @"'The Set Target Positions Cannot be Reached. The set target positions reside outside the admissible working space."},
            {33117, @"'Discontinuity in ACS axes detected. Discontinuity in ACS axes detected."},
            {33120, @"'Circle Specification in Path is invalid. The specification of a circle segment in the programmed interpolated path (e.g. via MC_MovePath) has an invalid or ambiguous description. Probably its center cannot be determined reliably."},
            {33121, @"'Maximum stream lines reached. The maximum number of stream lines is limited. Please refer to function block documentation for details."},
            {33123, @"'Invalid First Segment. The corresponding element can only be analyzed with a well-defined start point."},
            {33124, @"'Invalid auxiliary point. The auxiliary point is not well-defined."},
            {33126, @"'Invalid parameter for GapControlMode. Invalid parameter for GapControlMode, most likely in combination with the group parameter GapControlDirection."},
            {33127, @"'Group got unsupported Axis Event (e.g. State Change). Group got unsupported Axis Event (e.g. State Change e.g. triggered by a Single Axis Reset)."},
            {33128, @"'Unsupported Compensation Type. The compensation type was either not set or is not supported by the addressed object."},
            {33129, @"'Master axis does not exist or cannot be used."},
            {33130, @"'Invalid or Missing Tracking Transformation. This error occurs at MC_TrackConveyorBelt if at the CoordTransform input an invalid object ID is used or the object ID points to an object that is not supported as coordinate transformation."},
            {33131, @"'Position is not on Track. Either Track cannot be activated because Actual Position is not on Track, or Target Position is not on Active Track or TrackPart"},
            {33132, @"'Axis does not have an activated track."},
            {33133, @"'Invalid Compensation ObjectId. An Object with this ObjectId does not exist or it is not of the right type (has to be a compensation)."},
            {33134, @"'Axis is in error because axis was not in Target when InTargetAlarm Timer expired."},
            {33135, @"'Coupling would cause a cyclic dependency of axis (e.g. via MC_GearInPos)."},
            {33136, @"'Axis was not added to an axes group, the command is not valid."},
            {33151, @"'Drive has invalid State."},
            {33153, @"'Parameter for gap control are invalid with the current configuration. Function block with gap control was issued to an axis that is not in a CA group"},
            {33154, @"'Software position limit violation. Software position limits of at least one axis have been or would have been violated by a command."},
            {33155, @"'Target position is not reachable. There is no path available to the target position or target position is unreachable in general."},
            {33157, @"'The mover or one of its relevant coordinates is busy. Either the whole mover or at least of its coordinates relevant to the command are busy."},
            {33158, @"'A collision has occured or would occur. Either a collision has occurred or would occur if the command was executed."},
            {33159, @"'Invalid Track Specification."},
            {33160, @"'Command not allowed in track state."},
            {33161, @"'Invalid Reference passed to Function Block. An invalid reference (or pointer) was used in a function block call. This can happen if a reference type is used before it was initialized."},
            {33162, @"'Path is locked against modifications. The path was locked to further changes. However, it might be resettable."},
            {36664, @"'Internal Error."},
            {36665, @"'Internal Error."},
            {36666, @"'Internal Error."},
            {36667, @"'Internal Error."},
            {36668, @"'Internal Error."},
            {36669, @"'Internal Error."},
            {36670, @"'Internal Error."},
            {36671, @"'Internal Error."},
            {36672, @"'Internal Error."},
            {36673, @"'Internal Error."},
            {36674, @"'Internal Error."},
            {36675, @"'Internal Error."},
            {36676, @"'Internal Error."},
            {36677, @"'Internal Error."},
            {36678, @"'Internal Error."},
            {36679, @"'Internal Error."},
            {36680, @"'Internal Error."},
            {36681, @"'Internal Error."},
            {36682, @"'Internal Error."},
            {36683, @"'Internal Error."},
            {36684, @"'Internal Error."},
            {36685, @"'Internal Error."},
            {36686, @"'Internal Error."},
            {36687, @"'Internal Error."},
            {36688, @"'Internal Error."},
            {36689, @"'Internal Error."},
            {36690, @"'Internal Error."},
            {36691, @"'Internal Error."},
            {36692, @"'Internal Error."},
            {36693, @"'Internal Error."},
            {36694, @"'Internal Error."},
            {36695, @"'Internal Error."},
            {36696, @"'Internal Error."},
            {36697, @"'Internal Error."},
            {36698, @"'Internal Error."},
            {36699, @"'Internal Error."},
            {36700, @"'Internal Error."},
            {36701, @"'Internal Error."},
            {36702, @"'Internal Error."},
            {36703, @"'Internal Error."},
            {36704, @"'Internal Error."},
            {36705, @"'Internal Error."},
            {36706, @"'Internal Error."},
            {36707, @"'Internal Error."},
            {36708, @"'Internal Error."},
            {36709, @"'Internal Error."},
            {36710, @"'Internal Error."},
            {36711, @"'Internal Error."},
            {36712, @"'Internal Error."},
            {36713, @"'Internal Error."},
            {36714, @"'Internal Error."},
            {36715, @"'Internal Error."},
            {36716, @"'Internal Error."},
            {36717, @"'Internal Error."},
            {36718, @"'Internal Error."},
            {36719, @"'Internal Error."},
            {36720, @"'Internal Error."},
            {36721, @"'Internal Error."},
            {36722, @"'Internal Error."},
            {36723, @"'Internal Error."},
            {36724, @"'Internal Error."},
            {36725, @"'Internal Error."},
            {36726, @"'Internal Error."},
            {36727, @"'Internal Error."},
            {36728, @"'Internal Error."},
            {36729, @"'Internal Error."},
            {36730, @"'Internal Error."},
            {36731, @"'Internal Error."},
            {36732, @"'Internal Error."},
            {36733, @"'Internal Error."},
            {36734, @"'Internal Error."},
            {36735, @"'Internal Error."},
            {36736, @"'Internal Error."},
            {36737, @"'Internal Error."},
            {36738, @"'Internal Error."},
            {36739, @"'Internal Error."},
            {36740, @"'Internal Error."},
            {36741, @"'Internal Error."},
            {36742, @"'Internal Error."},
            {36743, @"'Internal Error."},
            {36744, @"'Internal Error."},
            {36745, @"'Internal Error."},
            {36746, @"'Internal Error."},
            {36747, @"'Internal Error."},
            {36748, @"'Internal Error."},
            {36749, @"'Internal Error."},
            {36750, @"'Internal Error."},
            {36751, @"'Internal Error."},
            {36752, @"'Internal Error."},
            {36753, @"'Internal Error."},
            {36754, @"'Internal Error."},
            {36755, @"'Internal Error."},
            {36756, @"'Internal Error."},
            {36757, @"'Internal Error."},
            {36758, @"'Internal Error."},
            {36759, @"'Internal Error."},
            {36760, @"'Internal Error."},
            {36761, @"'Internal Error."},
            {36762, @"'Internal Error."},
            {36763, @"'Internal Error."},
            {36764, @"'Internal Error."},
            {36765, @"'Internal Error."},
            {36766, @"'Internal Error."},
            {36767, @"'Internal Error."},
            {36768, @"'Internal Error."},
            {36769, @"'Internal Error."}
            //{36770, @"'Internal Error."},
            //{36770, @"'Internal Error."},
            //{36771, @"'Internal Error."},
            //{36771, @"'Internal Error."},
            //{36772, @"'Internal Error."},
            //{36772, @"'Internal Error."},
            //{36773, @"'Internal Error."},
            //{36773, @"'Internal Error."},
            //{36774, @"'Internal Error."},
            //{36774, @"'Internal Error."},
            //{36775, @"'Internal Error."},
            //{36775, @"'Internal Error."},
            //{36776, @"'Internal Error."},
            //{36776, @"'Internal Error."},
            //{36777, @"'Internal Error."},
            //{36777, @"'Internal Error."},
            //{36778, @"'Internal Error."},
            //{36778, @"'Internal Error."},
            //{36779, @"'Internal Error."},
            //{36779, @"'Internal Error."},
            //{36780, @"'Internal Error."},
            //{36780, @"'Internal Error."},
            //{36781, @"'Internal Error."},
            //{36781, @"'Internal Error."},
            //{36782, @"'Internal Error."},
            //{36782, @"'Internal Error."},
            //{36783, @"'Internal Error."},
            //{36783, @"'Internal Error."},
            //{36784, @"'Internal Error."},
            //{36784, @"'Internal Error."},
            //{36785, @"'Internal Error."},
            //{36785, @"'Internal Error."},
            //{36786, @"'Internal Error."},
            //{36786, @"'Internal Error."},
            //{36787, @"'Internal Error."},
            //{36787, @"'Internal Error."},
            //{36788, @"'Internal Error."},
            //{36788, @"'Internal Error."},
            //{36789, @"'Internal Error."},
            //{36789, @"'Internal Error."},
            //{36790, @"'Internal Error."},
            //{36790, @"'Internal Error."},
            //{36791, @"'Internal Error."},
            //{36791, @"'Internal Error."},
            //{36792, @"'Internal Error."},
            //{36792, @"'Internal Error."},
            //{36793, @"'Internal Error."},
            //{36793, @"'Internal Error."},
            //{36794, @"'Internal Error."},
            //{36794, @"'Internal Error."},
            //{36795, @"'Internal Error."},
            //{36795, @"'Internal Error."},
            //{36796, @"'Internal Error."},
            //{36796, @"'Internal Error."},
            //{36797, @"'Internal Error."},
            //{36797, @"'Internal Error."},
            //{36798, @"'Internal Error."},
            //{36798, @"'Internal Error."},
            //{36799, @"'Internal Error."},
            //{36799, @"'Internal Error."},
            //{36800, @"'Internal Error."},
            //{36800, @"'Internal Error."},
            //{36801, @"'Internal Error."},
            //{36801, @"'Internal Error."},
            //{36802, @"'Internal Error."},
            //{36802, @"'Internal Error."},
            //{36803, @"'Internal Error."},
            //{36803, @"'Internal Error."},
            //{36804, @"'Internal Error."},
            //{36804, @"'Internal Error."},
            //{36805, @"'Internal Error."},
            //{36805, @"'Internal Error."},
            //{36806, @"'Internal Error."},
            //{36806, @"'Internal Error."},
            //{36807, @"'Internal Error."},
            //{36807, @"'Internal Error."},
            //{36808, @"'Internal Error."},
            //{36808, @"'Internal Error."},
            //{36809, @"'Internal Error."},
            //{36809, @"'Internal Error."},
            //{36810, @"'Internal Error."},
            //{36810, @"'Internal Error."},
            //{36811, @"'Internal Error."},
            //{36811, @"'Internal Error."},
            //{36812, @"'Internal Error."},
            //{36812, @"'Internal Error."},
            //{36813, @"'Internal Error."},
            //{36813, @"'Internal Error."},
            //{36814, @"'Internal Error."},
            //{36814, @"'Internal Error."},
            //{36815, @"'Internal Error."},
            //{36815, @"'Internal Error."},
            //{36816, @"'Internal Error."},
            //{36816, @"'Internal Error."},
            //{36817, @"'Internal Error."},
            //{36817, @"'Internal Error."},
            //{36818, @"'Internal Error."},
            //{36818, @"'Internal Error."},
            //{36819, @"'Internal Error."},
            //{36819, @"'Internal Error."},
            //{36820, @"'Internal Error."},
            //{36820, @"'Internal Error."},
            //{36821, @"'Internal Error."},
            //{36821, @"'Internal Error."},
            //{36822, @"'Internal Error."},
            //{36822, @"'Internal Error."},
            //{36823, @"'Internal Error."},
            //{36823, @"'Internal Error."},
            //{36824, @"'Internal Error."},
            //{36824, @"'Internal Error."},
            //{36825, @"'Internal Error."},
            //{36825, @"'Internal Error."},
            //{36826, @"'Internal Error."},
            //{36826, @"'Internal Error."},
            //{36827, @"'Internal Error."},
            //{36827, @"'Internal Error."},
            //{36828, @"'Internal Error."},
            //{36828, @"'Internal Error."},
            //{36829, @"'Internal Error."},
            //{36829, @"'Internal Error."},
            //{36830, @"'Internal Error."},
            //{36830, @"'Internal Error."},
            //{36831, @"'Internal Error."},
            //{36831, @"'Internal Error."},
            //{36832, @"'Internal Error."},
            //{36832, @"'Internal Error."},
            //{36833, @"'Internal Error."},
            //{36833, @"'Internal Error."},
            //{36834, @"'Internal Error."},
            //{36834, @"'Internal Error."},
            //{36835, @"'Internal Error."},
            //{36835, @"'Internal Error."},
            //{36836, @"'Internal Error."},
            //{36836, @"'Internal Error."},
            //{36837, @"'Internal Error."},
            //{36837, @"'Internal Error."},
            //{36838, @"'Internal Error."},
            //{36838, @"'Internal Error."},
            //{36839, @"'Internal Error."},
            //{36839, @"'Internal Error."},
            //{36840, @"'Internal Error."},
            //{36840, @"'Internal Error."},
            //{36841, @"'Internal Error."},
            //{36841, @"'Internal Error."},
            //{36842, @"'Internal Error."},
            //{36842, @"'Internal Error."},
            //{36843, @"'Internal Error."},
            //{36844, @"'Internal Error."},
            //{36845, @"'Internal Error."},
            //{36846, @"'Internal Error."},
            //{36847, @"'Internal Error."},
            //{36848, @"'Internal Error."},
            //{36849, @"'Internal Error."},
            //{36850, @"'Internal Error."},
            //{36851, @"'Internal Error."},
            //{36852, @"'Internal Error."},
            //{36853, @"'Internal Error."},
            //{36854, @"'Internal Error."},
            //{36855, @"'Internal Error."},
            //{36856, @"'Internal Error."},
            //{36857, @"'Internal Error."},
            //{36858, @"'Internal Error."},
            //{36859, @"'Internal Error."},
            //{36860, @"'Internal Error."},
            //{36861, @"'Internal Error."},
            //{36862, @"'Internal Error."}
};
        //public static IDictionary<int, string> AxisErrors;
        //public static IDictionary<int, string> DriveErrors;
        //public static IDictionary<int, string> EncoderErrors;
        //public static IDictionary<int, string> ControlerErrors;
        //public static IDictionary<int, string> GeneralNcErrors;




        //static Errors()
        //{
        //    GeneralNcErrors = new Dictionary<int, string>()
        //    {

        //        {16384,@"Internal error Internal system error in the NC on ring 0, no further details."},
        //        {16385,@"Memory error The ring-0 memory management is not providing the required memory. This is usually a result of another error, as a result of which the controller will halt normal operation (now if not before)."},
        //        {16386,@"Nc retain data error (persistent data) Error while loading the Nc retain data. The axes concerned are no longer referenced (status flag Homed is set to FALSE).
        //        Possible reasons are:
        //        - Nc retain data not found
        //        - Nc retain data expired (old backup data)
        //        - Nc retain data corrupt or inconsistent"},
        //        {16387,@"Parameter for Monitoring the NC Setpoint Issuing is Invalid
        //        The parameter for activating or deactivating the function “cyclic monitoring of NC setpoint issuing on continuity and consistency” is invalid. (Special function.)"},
        //        {16388,@"External Error
        //        This error code can be set by an external module (e.g. third-party module) or can be set when an external module exhibits an error."},
        //        {16400,@"Channel identifier not allowed Either an unacceptable value (not 1...255) has been used, or a channel that does not exist in the system has been named."},
        //        {16401,@"Group identifier not allowed Either an unacceptable value (not 1...255) has been used, or a group that does not exist in the system has been named."},
        //        {16402,@"Axis identifier not allowed Either an unacceptable value (not 1...255) has been used, or an axis that does not exist in the system has been named."},
        //        {16403,@"Encoder identifier not allowed Either an unacceptable value (not 1...255) has been used, or a encoder that does not exist in the system has been named."},
        //        {16404,@"Controller identifier not allowed Either an unacceptable value (not 1...255) has been used, or a controller that does not exist in the system has been named."},
        //        {16405,@"Drive identifier not allowed Either an unacceptable value (not 1...255) has been used, or a drive that does not exist in the system has been named."},
        //        {16406,@"Table identifier not allowed Either an unacceptable value (not 1...255) has been used, or a table that does not exist in the system has been named."},
        //        {16416,@"No process image No PLC-axis interface during creation of an axis."},
        //        {16417,@"No process image No axis-PLC interface during creation of an axis."},
        //        {16418,@"No process image No encoder-I/O interface during creation of an axis."},
        //        {16419,@"No process image No I/O-encoder interface during creation of an axis."},
        //        {16420,@"No process image No drive-I/O interface during creation of an axis."},
        //        {16421,@"No process image No I/O-drive interface during creation of an axis."},
        //        {16432,@"Coupling type not allowed Unacceptable master/slave coupling type."},
        //        {16433,@"Axis type not allowed Unacceptable type specification during creation of an axis."},
        //        {16434,@"Unknown Channel Type - The NC channel type is unknown. Known types are e.g. an NCI channel, a FIFO channel, etc.."},
        //        {16448,@"Axis is incompatible Axis is not suitable for the intended purpose. A high speed/low speed axis, for example, cannot function as a slave in an axis coupling."},
        //        {16464,@"Channel not ready for operation The channel is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
        //        {16465,@"Group not ready for operation The group is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
        //        {16466,@"Axis not ready for operation The axis is not complete, and is therefore not ready for operation. This is usually a consequence of problems at system start-up."},
        //        {16480,@"Channel exists The channel that is to be created already exists."},
        //        {16481,@"Group exists The group that is to be created already exists."},
        //        {16482,@"Axis exists The axis that is to be created already exists."},
        //        {16483,@"Table exists The table that is to be created already exists, resp. it is tried internally to use an already existing table id ( e.g. for the universal flying saw)."},
        //        {16496,@"Axis index not allowed The location within the channel specified for an axis is not allowed."},
        //        {16497,@"Axis index not allowed The location within the group specified for an axis is not allowed."},
        //    };
        //    DriveErrors = new Dictionary<int, string>()
        //    {
        //        {17920,@"Drive ID not allowed The value for the drive ID is not allowed because, for example, it is already assigned, or is zero, or is greater than 255."},
        //        {17921,@"Drive type not allowed The value for the drive type is not allowed, because it is not defined."},
        //        {17922,@"Drive operation mode not allowed The value for the drive operation mode is not allowed (mode 1: standard). "},
        //        {17923,@"Motor polarity inverted? The flag for the motor polarity is not allowed. Flag 0: positive motor polarity Flag 1: negative motor polarity."},
        //        {17924,@"Drift compensation/speed offset (DAC offset) The value for the drift compensation (DAC offset) is not allowed."},
        //        {17925,@"Reference velocity (velocity pre-control) The value for the reference velocity (also called velocity pre-control) is not allowed."},
        //        {17926,@"Reference output in percent The value for the reference output in percent is not allowed. The value 1.0 (100 %) usually corresponds to a voltage of 10.0 V. "},
        //        {17927,@"Quadrant compensation factor The value for the quadrant compensation factor is not allowed. Value range: [0.0, 100.0]"},
        //        {17928,@"Velocity reference point in percent The value for the velocity reference point in percent is not allowed. The value 1.0 corresponds to 100 percent."},
        //        {17929,@"Output reference point in percent The value for the output reference point in percent is not allowed. The value 1.0 corresponds to 100 percent."},
        //        {17930,@"Minimum or maximum output limits (output limitation) The value for the minimum and/or maximum output limit is not allowed. This will happen if the value range is exceeded, the maximum limit is smaller than the minimum limit, or the distance between the minimum and maximum limits is zero. The minimum limit is initially set to –1.0 (-100 percent) and the maximum limit to 1.0 (100 percent). Value range: [-1.0, 1.0]"},
        //        {17931,@"Parameter Maximum value for output is not allowed. The value for the maximum number of output digits of the drive (maximum output value) is not allowed. Depending on the interface used (e.g. position, velocity or torque/current). A velocity interface is often a signed 16 bit output value (± 32767). Value range: [0x000000FF .. 0xFFFFFFFF]"},
        //        {17932,@"Parameter Internal Drive Control Word is not allowed. The value as internal Drive Control Word for the NC is not allowed. This contains information from the System Manager to the NC, which is evaluated by the NC at the TC start. Value range: [>0]"},
        //        {17933,@"Parameter Internal timer for RESET behavior Drive is not allowed. The special parameter that influences the internal time behavior between NC Drive and the IO Drive (servo drive) is not allowed. Value range: [>5]"},
        //        {17934,@"Parameter Master Motion Controller ID is not allowed. The Master Motion Controller ID parameter is not allowed for a further NC Motion Controller in slave mode. An additional NC Motion Controller in slave mode can be used if it is one and the same drive device to which different NC information for different operation modes is connected (e.g. velocity mode and torque mode).
        //        Note: This parameter is not directly accessible by the user, but can only be influenced indirectly by configuring additional NC Motion Controllers below the NC axis."},
        //        {17935,@"Drive torque output scaling not allowed The value is not allowed as drive torque output scaling (rotary motor) or as force output scaling (linear motor)."},
        //        {17936,@"Drive velocity output scaling not allowed The value is not allowed as drive velocity output scaling."},
        //        {17937,@"Profi Drive DSC proportional gain Kpc (controller) not allowed Positions The value for the Profi Drive DSC position control gain (Kpc factor) is not allowed."},
        //        {17938,@"Table ID is not allowed The value for the table ID is not allowed."},
        //        {17939,@"Table interpolation type is not allowed The value is not allowed as the table interpolation type."},
        //        {17940,@"Output offset in percent is not allowed The value is not allowed as an output offset in percent (+/- 1.0)."},
        //        {17941,@"Profi Drive DSC scaling for calculation of Xerr (controller) not allowed Positions The value is not allowed as Profi Drive DSC scaling for the calculation of 'Xerr'."},
        //        {17942,@"Drive acceleration output scaling not allowed The value is not allowed as drive acceleration/deceleration output scaling."},
        //        {17943,@"Drive position output scaling not allowed The value is not allowed as drive position output scaling."},
        //        {17944,@"Parameter Dead time compensation mode (Motion Controller) is invalid
        //        The parameter for the dead time compensation mode of the NC Motion Controller is invalid (OFF, ON with velocity, ON with velocity and acceleration)."},
        //        {17945,@"Parameter Control bits of the dead time compensation (Motion Controller) is invalid
        //        The parameter for the Control bits of the dead time compensation of the NC Motion Controller is invalid (e.g. relative or absolute time interpretation)."},
        //        {17946,@"Parameter time shift of dead time compensation mode (Motion Controller) is invalid
        //        The parameter for the time shift of the dead time compensation (Time Shift in nanoseconds) of the NC Motion Controller is invalid."},
        //        {17947,@"Parameter Output delay velocity interface Motion Controller is invalid
        //        The parameter for an optional output delay in the velocity interface to the Motion Controller is invalid (Delay Generator Velocity). The maximum permitted delay time must be less than 100 times the NC SAF cycle time."},
        //        {17948,@"Drive filter type not allowed for command variable filter for the output position The value is not allowed as a drive filter type for the smoothing of the output position (command variable filter for the set position)."},
        //        {17949,@"Drive filter time not allowed for command variable filter for the output position The value is not allowed as a drive filter time for the smoothing of the output position (command variable filter for the set position)."},
        //        {17950,@"Drive filter order not allowed for command variable filter for the output position The value is not allowed as a drive filter order (P-Tn) for the smoothing of the output position (command variable filter for the set position)."},
        //        {17952,@"Bit mask for stepper motor cycle not allowed A value of the different stepper motor masks is not allowed for the respective cycle."},
        //        {17953,@"Bit mask for stepper motor holding current not allowed The value for the stepper motor holding mask is not allowed."},
        //        {17954,@"Scaling factor for actual torque (actual current) not allowed The value is not allowed as a scaling factor for the actual torque (or actual current)."},
        //        {17955,@"Filter time for actual torque is not allowed The value is not allowed as a filter time for the actual torque (or the actual current) (P-T1 filter)."},
        //        {17956,@"Filter time for the temporal derivation of the actual torque is not allowed The value is not allowed as a filter time for the temporal derivation of the actual torque (or actual current) (P-T1 filter)."},
        //        {17957,@"Parameter Drive operation mode is invalid
        //        The parameter for the drive operation mode (motion controller operation mode: position mode, velocity mode, torque mode, ...) is invalid. It is possible that a NC operation mode changeover has been attempted or that an attempt was made to activate a preconfigured operation mode during the TC system startup.
        //        Notes: The generic operation modes defined in NC are implemented by NC in a drive-specific manner, i.e. in particular for the protocols SERCOS/ SoE and CANopen/ CoE (DS402). Here, protocol-specific, drive-specific or even vendor-specific features must be taken into account (e.g. with SERCOS/ SoE, predefined operation modes can only be activated at runtime in the SERCOS parameters S-0-0032 to S-0-0035). Furthermore, not every generic NC operation mode can be converted into a drive-specific operation mode (there may be gaps in the specification here).
        //        The generic NC operation mode 0 is a special case. This value is used as an identifier to activate a NC default mode (if this identifier is known to the NC)."},
        //        {17958,@"Motion Controller function is not supported
        //        A Motion Controller functionality has been triggered that is not enabled for use or is not implemented (e.g. writing or reading a drive operation mode that is not supported by certain Motion Controllers). It is also possible that this functionality is only temporarily unavailable (e.g. because the drive device is in error state or a drive enable is missing)."},
        //        {17959,@"DRIVEOPERATIONMODEBUSY. The activation of the drive operation mode failed, because another object with OID… is already using this interface."},
        //        {17960,@"Drive operation mode changeover is not configured or the desired drive operation mode cannot be found
        //        No drive operation mode changeover has been configured, and in this respect no reading or writing of a drive operation mode is possible. Or the desired drive operation mode has not been found in the list of predefined drive operation modes (e.g. for SoE/ SERCOS).
        //        Note for CoE Motion Controllers: reading or writing the CoE Motion Controller operation mode is only possible if the CoE objects 0x6060 Modes of operation and 0x6061 Modes of operation display are in the cyclic process data (PDO list) and a valid default operation mode has been configured.
        //        Note for SoE Motion Controllers: reading or writing the current SoE Motion Controller operation mode is only possible if this operation mode has been predefined in one of the SoE parameters S-0-0032 to S-0-0035."},
        //        {17961,@"Feedback drive operation mode changeover
        //        During drive operation mode changeover, the requested operation mode was not consistently reported back within the monitoring time of 8 cycles.
        //        Note for CoE Motion Controllers: reading or writing the CoE Motion Controller operation mode is only possible if the CoE objects 0x6060 Modes of operation and 0x6061 Modes of operation display are in the cyclic process data (PDO list) and a valid default operation mode has been configured.
        //        Note for SoE Motion Controllers: reading or writing the current SoE Motion Controller operation mode is only possible if this operation mode has been predefined in one of the SoE parameters S-0-0032 to S-0-0035."},
        //        {17968,@"Overtemperature Overtemperature was detected or reported in the drive or terminal."},
        //        {17969,@"Undervoltage Undervoltage was detected or reported in the drive or terminal."},
        //        {17970,@"Open circuit in phase A An open circuit in phase A was detected or reported in the drive or terminal."},
        //        {17971,@"Open circuit in phase B An open circuit in phase B was detected or reported in the drive or terminal."},
        //        {17972,@"Overcurrent in phase A Overcurrent was detected or reported in phase A in the drive or terminal."},
        //        {17973,@"Overcurrent in phase B Overcurrent was detected or reported in phase B in the drive or terminal."},
        //        {17974,@"Torque overload (stall) A torque overload (stall) was detected or reported in the drive or terminal."},
        //        {17984,@"Drive initialization Drive has not been initialized. Although the drive has been created, the rest of the initialization has not been performed (1. Initialization of drive I/O, 2. Initialization of drive, 3. Reset drive)."},
        //        {17985,@"Axis address Drive does not know its axis, or the axis address has not been initialized."},
        //        {17986,@"Address I/O input structure Drive has no valid I/O input address in the process image."},
        //        {17987,@"Address I/O output structure Drive has no valid I/O output address in the process image."},
        //        {18000,@"Drive hardware not ready to operate The drive hardware is not ready for operation. This can be caused by the following reasons:
        //        - the drive is in error state (hardware error)
        //        - the drive is in the start-up phase (e.g. after an axis reset preceded by a hardware error)
        //        - the drive lacks the controller enable (ENABLE)
        //        Note: The time required for the start-up of a drive after a hardware error can be in the range of several seconds."},
        //        {18001,@"Error in the cyclic communication of the drive (Life Counter) .Reasons for this could be an interrupted fieldbus or a drive that is in the error state."},
        //        {18002,@"Changing the table ID when active controller enable is not allowed.Changing (deselecting, selecting) the characteristic curve table ID is not allowed when the controller enable for the axis is active."},
        //        {18005,@"Invalid I/O data for more than 'n' continuous NC cyclesThe axis (encoder or drive) has detected invalid I/O data for more than 'n' continuous NC cycles (NC SAF task) (e.g. n=3).
        //        As a consequence it is possible that the encoder referencing flag is reset to FALSE (i.e. the encoder gets the status unreferenced).EtherCAT fieldbus: working counter error ('WCState')
        //        Lightbus fieldbus: CDL state error ('CdlState')"},
        //    };
        //    ControlerErrors = new Dictionary<int, string>()
        //    {

        //        {17664,@"Controller ID not allowed The value for the controller ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is greater than 255."},
        //        {17665,@"Controller type not allowed The value for the controller type is unacceptable because it is not defined. Type 1: P-controller (position) . . . Type 7: High/low speed controller Type 8: Stepper motor controller Type 9: Sercos controller"},
        //        {17666,@"Controller operating mode not allowed The value for the controller operating mode is not allowed."},
        //        {17667,@"Weighting of the velocity pre-control not allowed The value for the percentage weighting of the velocity pre-control is not allowed. The parameter is pre-set to 1.0 (100%) as standard."},
        //        {17668,@"Following error monitoring (position) not allowed The value for the activation of the following error monitoring is not allowed."},
        //        {17669,@"Following error (velocity) not allowed The value for the activation of the following error monitoring (velocity) is not allowed."},
        //        {17670,@"Following error window (position) not allowed The value for the following error window (maximum allowable following error) is not allowed."},
        //        {17671,@"Following error filter time (position) not allowed The value for the following error filter time (position) is not allowed."},
        //        {17672,@"Following error window (velocity) not allowed The value for the following error window (velocity) is not allowed."},
        //        {17673,@"Following error filter time (velocity) not allowed The value for the following error filter time (velocity) is not allowed."},
        //        {17674,@"“Controller Output Limitation” Improper
        //        The value for output limitation of the controller at the overall setpoint quantity is improper. The presetting amounts to 0.5 (50 percent). Typically, this parameter is at work if to the motion controller device the velocity interface has been parameterized and the NC performs position control of the position on the controller."},
        //        {17680,@"Proportional gain Kv or Kp (controller) not allowed position The value for the proportional gain (Kv factor or Kp factor) is not allowed."},
        //        {17681,@"Integral-action time Tn (controller) not allowed position The value for the integral-action time is not allowed (I proportion of the PID T1 controller)."},
        //        {17682,@"Derivative action time Tv (controller) not allowed position The value for the derivative action time is not allowed (D proportion of the PID T1 controller)."},
        //        {17683,@"Damping time Td (controller) not allowed position The value for the damping time is not allowed (D proportion of the PID T1 controller). Suggested value: 0.1 * Tv"},
        //        {17684,@"Activation of the automatic offset compensation not allowed Activation of the automatic offset compensation is only possible for certain types of controller (with no I component)."},
        //        {17685,@"Additional proportional gain Kv or Kp (controller) not allowed position The value for the second term of the proportional gain (Kv factor or Kp factor) is not allowed."},
        //        {17686,@"Reference velocity for additional proportional gain Kv or Kp (controller) not allowed position The value for the reference velocity percentage data entry, to which the additional proportional gain is applied, is not allowed. The standard setting for the parameter is 0.5 (50%)."},
        //        {17687,@"Proportional gain Pa (proportion) not allowed acceleration The value for the proportional gain (Pa factor) is not allowed."},
        //        {17688,@"Proportional gain Kv (velocity controller) not allowed The value for the proportional gain (Kv factor) is not allowed."},
        //        {17689,@"“Reset time Tn (velocity controller) not allowed” The value for the integral-action time is not allowed (I proportion of the PID T1 controller)."},
        //        {17690,@"Reserved"},
        //        {17691,@"Reserved"},
        //        {17692,@"“Velocity Filter Time” Improper
        //        The parameter for velocity filter time in seconds is improper (P-T1 filter). This filter can be used within the NC for filtering an actual velocity or a velocity difference (velocity error = setpoint velocity - actual velocity) in special NC controllers (e.g. within the torque interface)."},
        //        {17693,@"„Dead zone not allowed“ The value for the dead zone from the position error or the velocity error (system deviation) is not allowed (only for complex controller with velocity or torque interface)."},
        //        {17695,@"“Proportionality Factor Kcp” Improper
        //        The parameter for the “proportional factor Kcp” of the slave coupling differential control is improper."},
        //        {17696,@"”Rate time Tv (velocity controller) not allowed” The value for the derivative action time is not allowed (D proportion of the PID T1 controller)."},
        //        {17697,@"Damping time Td (velocity controller) not allowed The value for the damping time is not allowed (D proportion of the PID T1 controller). Suggested value: 0.1 * Tv"},
        //        {17698,@"“Limitation of the I Part” Improper
        //        The parameter for limiting the I part of a PI or PID controller is improper. This inner state quantity can be limited in percent (1.0 refers to 100 percent)."},
        //        {17699,@"“Limitation of the D Part” Improper
        //        The parameter for limitation of the D part of a PI or PID controller is improper. This inner state quantity may be limited in percent (1.0 refers to 100 percent)."},
        //        {17700,@"Parameter “Switching Off the I Part During Motion” is Improper
        //        The boolean parameter for switching off the I part during an active positioning is improper."},
        //        {17701,@"Parameter “Filter Time for P-T2 Filter” Improper
        //        The time T0 in seconds is as filter time for the velocity controller P-T2 element improper.
        //        The filter time has to be smaller than twice the NC-SAF cycle time."},
        //        {17702,@"Velocity Observer: “Parameterized Mode” is Improper
        //        The parameterized mode (0=OFF, 1=LUENBERGER) for the special NC controller velocity observer within the torque interface is improper."},
        //        {17703,@"Velocity Observer: “Motor Torque Constant Kt or Kf” is Improper
        //        The parameter for the motor torque constant Kt (rotational motor) or Kf (linear motor) of the special NC controller velocity observer within the torque interface is improper."},
        //        {17704,@"Velocity Observer: “Motor Moment of Inertia JM” is Improper
        //        The parameter for the motor moment of inertia JM of the special NC controller velocity observer within the torque interface is improper."},
        //        {17705,@"Velocity Observer: “Band Width f0” is Improper
        //        The parameter for the band width f0 of the special NC controller velocity observer within the torque interface is improper. The band width has to be smaller than the reciprocal value of six times the NC cycle time (f0 < 1/(6*T))."},
        //        {17706,@"Velocity Observer: “Correction Factor kc” is Improper
        //        The parameter for the correction factor kc of the special NC controller velocity observer within the torque interface is improper. The correction factor kc implements the relation between current and acceleration or angular acceleration."},
        //        {17707,@"Velocity Observer: “Time Constant T for First Order Filter” is Improper
        //        The time constant T for the first order velocity filter (PID-T2 or “Lead Lag”) of the specific NC controller velocity observer within the torque interface is improper. The correction factor kc implements the relation between current and acceleration or angular acceleration."},
        //        {17708,@"Velocity Observer: “Amplitude Damping d for Second Order Filter” is Improper
        //        The high pass/ low pass amplitude damping dHP or dTP for the second order velocity filter (“Bi-Quad”) of the special NC controller velocity observer within the torque interface is improper."},
        //        {17709,@"Velocity Observer: “Frequency fHP or Frequency fTP for Filters of Second Order” is Improper
        //        The high pass frequency fHP or the low pass frequency fTP for the second order velocity filter (“Bi-Quad”) of the specific NC controller velocity observer within the torque interface is improper."},
        //        {17728,@"Controller initialization Controller has not been initialized. Although the controller has been created, the rest of the initialization has not been performed (1. Initialization of controller, 2. Reset controller)."},
        //        {17729,@"Axis address Controller does not know its axis, or the axis address has not been initialized."},
        //        {17730,@"Drive address Controller does not know its drive, or the drive address has not been initialized."},
        //        {17744,@"Following error monitoring (position) With active following error monitoring (position) a following error exceedance has occurred, whose magnitude is greater than the following error window, and whose duration is longer than the parameterized following error filter time."},
        //        {17745,@"Following error monitoring (velocity) With active following error monitoring (velocity) a velocity following error exceedance has occurred, whose magnitude is greater than the following error window, and whose duration is longer than the parameterized following error filter time."},
        //        {17824,@"CONTROLERR_RANGE_AREA_ASIDE"},
        //        {17825,@"CONTROLERR_RANGE_AREA_BSIDE"},
        //        {17826,@"CONTROLERR_RANGE_QNENN"},
        //        {17827,@"CONTROLERR_RANGE_PNENN"},
        //        {17828,@"CONTROLERR_RANGE_AXISIDPRESP0"},

        //    };
        //    EncoderErrors = new Dictionary<int, string>()
        //    {
        //        {17408,@"Encoder ID not allowed The value for the encoder ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, or is bigger than 255."},
        //        {17409,@"Encoder type not allowed The value for the encoder type is unacceptable because it is not defined. Type 1: Simulation (incremental) Type 2: M3000 (24 bit absolute) Type 3: M31x0 (24 bit incremental) Type 4: KL5101 (16 bit incremental) Type 5: KL5001 (24 bit absolute SSI) Type 6: KL5051 (16 bit BISSI)"},
        //        {17410,@"Encoder mode The value for the encoder (operating) mode is not allowed. Mode 1: Determination of the actual position Mode 2: Determination of the actual position and the actual velocity (filter)"},
        //        {17411,@"Encoder counting direction inverted? The flag for the encoder counting direction is not allowed. Flag 0: Positive encoder counting direction Flag 1: Negative encoder counting direction"},
        //        {17412,@"Referencing status The flag for the referencing status is not allowed. Flag 0: Axis has not been referenced Flag 1: Axis has been referenced"},
        //        {17413,@"Encoder increments for each physical encoder rotation The value for the number of encoder increments for each physical rotation of the encoder is not allowed. This value is used by the software for the calculation of encoder overruns and underruns."},
        //        {17414,@"Scaling factor The value for the scaling factor is not allowed. This scaling factor provides the weighting for the conversion of an encoder increment (INC) to a physical unit such as millimeters or degrees."},
        //        {17415,@"Position offset (zero point offset) The value for the position offset of the encoder is not allowed. This value is added to the calculated encoder position, and is interpreted in the physical units of the encoder."},
        //        {17416,@"Modulo factor The value for the encoder's modulo factor is not allowed."},
        //        {17417,@"Position filter time The value for the actual position filter time is not allowed (P-T1 filter)."},
        //        {17418,@"Velocity filter time The value for the actual velocity filter time is not allowed (P-T1 filter)."},
        //        {17419,@"Acceleration filter time The value for the actual acceleration filter time is not allowed (P-T1 filter)."},
        //        {17420,@"Cycle time not allowed (INTERNAL ERROR) The value of the SAF cycle time for the calculation of actual values is not allowed (e.g. is less than or equal to zero)."},
        //        {17421,@"“Configuration of the selected units is invalid” E.g. settings for modulo position, velocity per minute etc. lead to an error."},
        //        {17422,@"Actual position correction / measurement system error correction The value for the activation of the actual position correction (measuring system error correction) is not allowed."},
        //        {17423,@"Filter time actual position correction The value for the actual position correction filter time is not allowed (P-T1 filter)."},
        //        {17424,@"Search direction for referencing cam inverted The value of the search direction of the referencing cam in a referencing procedure is not allowed. Value 0: Positive direction Value 1: Negative direction"},
        //        {17425,@"Search direction for sync pulse (zero pulse) inverted The value of the search direction of the sync pulse (zero pulse) in a referencing procedure is not allowed. Value 0: Positive direction Value 1: Negative direction"},
        //        {17426,@"Reference position The value of the reference position in a referencing procedure is not allowed."},
        //        {17427,@"Clearance monitoring between activation of the hardware latch and appearance of the sync pulse (NOT IMPLEMENTED) The flag for the clearance monitoring between activation of the hardware latch and occurrence of the sync/zero pulse (latch valid) is not allowed. Value 0: Passive Value 1: Active"},
        //        {17428,@"Minimum clearance between activation of the hardware latch and appearance of the sync pulse (NOT IMPLEMENTED) The value for the minimum clearance in increments between activation of the hardware latch and occurrence of the sync/zero pulse (latch valid) during a referencing procedure is not allowed."},
        //        {17429,@"External sync pulse (NOT IMPLEMENTED) The value of the activation or deactivation of the external sync pulse in a referencing procedure is not allowed. Value 0: Passive Value 1: Active"},
        //        {17430,@"Scaling of the noise rate is not allowed The value of the scaling (weighting) of the synthetic noise rate is not allowed. This parameter exists only in the simulation encoder and serves to produce a realistic simulation."},
        //        {17431,@"„Tolerance window for modulo-start“ The value for the tolerance window for the modulo-axis-start is invalid. The value must be greater or equal than zero and smaller than the half encoder modulo-period (e. g. in the interval [0.0,180.0) )."},
        //        {17432,@"„Encoder reference mode“ The value for the encoder reference mode is not allowed, resp. is not supported for this encoder type."},
        //        {17433,@"„Encoder evaluation direction“ The value for the encoder evaluation direction (log. counter direction) is not allowed."},
        //        {17434,@"„Encoder reference system“ The value for the encoder reference system is invalid (0: incremental, 1: absolute, 2: absolute+modulo)."},
        //        {17435,@"„Encoder position initialization mode“ When starting the TC system the value for the encoder position initialization mode is invalid."},
        //        {17436,@"„Encoder sign interpretation (UNSIGNED- / SIGNED- data type)“ The value for the encoder sign interpretation (data type) for the encoder the actual increment calculation (0: Default/not defined, 1: UNSIGNED, 2:/ SIGNED) is invalid."},
        //        {17437,@"“Homing Sensor Source” The value for the encoder homing sensor source is not allowed, resp. is not supported for this encoder type."},
        //        {17440,@"Software end location monitoring minimum not allowed The value for the activation of the software location monitoring minimum is not allowed."},
        //        {17441,@"Software end location monitoring maximum not allowed The value for the activation of the software location monitoring maximum is not allowed."},
        //        {17442,@"Actual value setting is outside the value range The set actual value function cannot be carried out, because the new actual position is outside the expected range of values."},
        //        {17443,@"Software end location minimum not allowed The value for the software end location minimum is not allowed."},
        //        {17444,@"Software end location maximum not allowed The value for the software end location maximum is not allowed."},
        //        {17445,@"„Filter mask for the raw data of the encoder is invalid“ The value for the filter mask of the encoder raw data in increments is invalid."},
        //        {17446,@"„Reference mask for the raw data of the encoder is invalid“ The value for the reference mask (increments per encoder turn, absolute resolution) for the raw data of the encoder is invalid. E.g. this value is used for axis reference sequence (calibration) with the reference mode Software Sync."},
        //        {17447,@"Parameter Dead Time Compensation Mode (Encoder) is Invalid
        //        The parameter for the mode of dead time compensation at the NC encoder is invalid (OFF, ON with velocity, ON with velocity and acceleration)."},
        //        {17448,@"Parameter “Control Bits of Dead Time Compensation” (Encoder) is Invalid
        //        The parameter for the control bits of dead time compensation at the encoder is invalid (e.g. relative or absolute time interpretation)."},
        //        {17449,@"Parameter “Time Related Shift of Dead Time Compensation Mode” (Encoder) is Invalid
        //        The parameter for time related shift of dead time compensation (time shift in nanoseconds) at the encoder is invalid."},
        //        {17456,@"Hardware latch activation (encoder) Activation of the encoder hardware latch was implicitly initiated by the referencing procedure. If this function has already been activated but a latch value has not yet become valid (latch valid), another call to the function is refused with this error."},
        //        {17457,@"External hardware latch activation (encoder) The activation of the external hardware latch (only available on the KL5101) is initiated explicitly by an ADS command (called from the PLC program of the Visual Basic interface). If this function has already been activated, but the latch value has not yet been made valid by an external signal (external latch valid), another call to the function is refused with this error."},
        //        {17458,@"External hardware latch activation (encoder) If a referencing procedure has previously been initiated and the hardware still signals a valid latch value (latch valid), this function must not be called. In practice, however, this error can almost never occur."},
        //        {17459,@"External hardware latch activation (encoder) If this function has already been initiated and the hardware is still signaling that the external latch value is still valid (extern latch valid), a further activation should not be carried out and the commando will be declined with an error (the internal handshake communication between NC and IO device is still active). In that case the validity of the external hardware latch would immediately be signaled, although the old latch value would still be present."},
        //        {17460,@"Encoder function not supported An encoder function has been activated that is currently not released for use, or which is not even implemented."},
        //        {17461,@"„Encoder function is already active“ An encoder function can not been activated because this functionality is already active."},
        //        {17472,@"Encoder initialization Encoder has not been initialized. Although the axis has been created, the rest of the initialization has not been performed (1. Initialization of axis I/O, 2. Initialization of axis, 3. Reset axis)."},
        //        {17473,@"Axis address The encoder does not have an axis, or the axis address has not been initialized."},
        //        {17474,@"I/O input structure address The drive does not have a valid I/O input address in the process image."},
        //        {17475,@"I/O output structure address The encoder does not have a valid I/O output address in the process image."},
        //        {17488,@"Encoder counter underflow monitoring The encoder's incremental counter has underflowed."},
        //        {17489,@"Encoder counter overflow monitoring The encoder's incremental counter has overflowed."},
        //        {17504,@"Minimum Software Position Limit (Axis Start)
        //        While monitoring of the minimum software position limit is active, an axis start has been performed towards a position that lies below the minimum software position limit."},
        //        {17505,@"Maximum Software Position Limit (Axis Start)
        //        While monitoring of the maximum software position limit is active, an axis start has been performed towards a position that lies above the maximum software position limit."},
        //        {17506,@"Minimum Software Position Limit (Positioning Process)
        //        While monitoring of the minimum software position limit is active, the actual position has fallen below the minimum software position limit. In case of servo axes, which are moved continuously, this limit is expanded by the magnitude of the parameterized following error position window."},
        //        {17507,@"Maximum Software Position Limit (Positioning Process)
        //        While monitoring of the maximum software position limit is active, the actual position has exceeded the maximum software position limit. In case of servo axes, which are moved continuously, this limit is expanded by the magnitude of the parameterized following error position window."},
        //        {17508,@"„Encoder hardware error“ The drive resp. the encoder system reports a hardware error of the encoder. An optimal error code is displayed in the message of the event log."},
        //        {17509,@"„Position initialization error at system start“ At the first initialization of the set position was this for all initialization trials (without over-/under-flow, with underflow and overflow ) out of the final position minimum and maximum."},
        //        {17510,@"Invalid IO data for more than n subsequent NC cycles (encoder)
        //        The axis (encoder) has detected for more than n subsequent NC cycles (NC SAF task) invalid encoder IO data (e.g. n=3). Typically, regarding an EtherCAT member it is about a Working Counter Error (WcState) what displays that data transfer between IO device and controller is disturbed.
        //        If this error is set for a longer period of time continuously, this situation can lead to losing the axis reference (the “homed” flag will be reset and the encoder will get the state “unreferenced”
        //        Possible reasons for this error: An EtherCAT slave may have left its OP state or there is a too high real time usage or a too high real time jitter."},
        //        {17511,@"Invalid Actual Position (Encoder)
        //        The IO device delivers an invalid actual position (for CANopen/CoE look at bit 13 of encoder state “TxPDO data invalid” or “invalid actual position value”)."},
        //        {17512,@"Invalid IO Input Data (Error Type 1)
        //        The monitoring of the “cyclic IO input counter” (2 bit counter) has detected an error. The input data has not been refreshed for at least 3 NC SAF cycles (the 2 bit counter displays a constant value for multiple NC SAF cycles, instead of incrementing by exactly one from cycle to cycle)."},
        //        {17513,@"Invalid IO Input Data (Error Type 2)
        //        The monitoring of the “cyclic IO input counter” (2 bit counter) has detected an error. The quality of input data based on this two bit counter is not sufficient (there is here a simple statistic evaluation that evaluates GOOD cases and BAD cases and in exceeding a special limit value leads to an error)."},
        //        {17520,@"SSI transformation fault or not finished The SSI transformation of the FOX 50 module was faulty for some NC-cycles or did not finished respectively."},
        //        {17570,@"ENCERR_ADDR_CONTROLLER"},
        //        {17571,@"ENCERR_INVALID_CONTROLLERTYPE"},



        //    };
        //    AxisErrors = new Dictionary<int, string>()
        //    {
        //        { 17152, @"Axis ID not allowed The value for the axis ID is not allowed, e.g. because it has already been assigned, is less than or equal to zero, is greater than 255, or does not exist in the current configuration." },
        //        { 17153, @"Axis type not allowed The value for the axis type is not allowed, because it is not defined. Type 1: Servo Type 2: Fast/creep Type 3: Stepper motor" },
        //        { 17158, @"Slow manual velocity not allowed The value for the slow manual velocity is not allowed." },
        //        { 17159, @"Fast manual velocity not allowed The value for the fast manual velocity is not allowed." },
        //        { 17160, @"High speed not allowed The value for the high speed is not allowed." },
        //        { 17161, @"Acceleration not allowed The value for the axis acceleration is not allowed." },
        //        { 17162, @"Deceleration not allowed The value for the axis deceleration is not allowed." },
        //        { 17163, @"Jerk not allowed The value for the axis jerk is not allowed." },
        //        { 17164, @"Delay time between position and velocity is not allowed (dead time compensation) The value for the delay time between position and velocity (dead time compensation) is not allowed." },
        //        { 17165, @"Override type not allowed The value for the velocity override type is not allowed, because it is not defined. Type 1: related to internal reduced velocity (default value) Type 2: related to original external start velocity" },
        //        { 17166, @"NCI: Velo-Jump-Factor not allowed
        //        An attempt was made to specify an invalid value for the velo-jump-factor. This parameter only works for TwinCAT NCI." },
        //        { 17167, @"NCI: Size of tolerance sphere for auxiliary axis invalid
        //        An attempt was made to specify an invalid value for the size of the tolerance sphere. This sphere affects only auxiliary axes!" },
        //        { 17168, @"NCI: Value for maximum deviation for auxiliary axis invalid
        //        An attempt was made to specify an invalid value for the value of the maximum deviation. This parameter affects only auxiliary axes!" },
        //        { 17170, @"Referencing velocity in direction of cam not allowed The value for the referencing velocity in the direction of the referencing cam is not allowed." },
        //        { 17171, @"Referencing velocity in sync direction not allowed The value for the referencing velocity in direction of the sync pulse (zero track) is not allowed." },
        //        { 17172, @"Pulse width in positive direction not allowed The value for the pulse width in positive direction is not allowed (pulsed operation). The use of the pulse width for positioning is chosen implicitly through the axis start type. Pulsed operation corresponds to positioning with a relative displacement that corresponds precisely to the pulse width." },
        //        { 17173, @"Pulse width in negative direction not allowed The value for the pulse width in negative direction is not allowed (pulsed operation). The use of the pulse width for positioning is chosen implicitly through the axis start type. Pulsed operation corresponds to positioning with a relative displacement that corresponds precisely to the pulse width." },
        //        { 17174, @"Pulse time in positive direction not allowed The value for the pulse width in positive direction is not allowed (pulsed operation)." },
        //        { 17175, @"Pulse time in negative direction not allowed The value for the pulse width in negative direction is not allowed (pulsed operation)." },
        //        { 17176, @"Creep distance in positive direction not allowed The value for the creep distance in positive direction is not allowed." },
        //        { 17177, @"Creep distance in negative direction not allowed The value for the creep distance in negative direction is not allowed." },
        //        { 17178, @"Braking distance in positive direction not allowed The value for the braking distance in positive direction is not allowed." },
        //        { 17179, @"Braking distance in negative direction not allowed The value for the braking distance in negative direction is not allowed." },
        //        { 17180, @"Braking time in positive direction not allowed The value for the braking time in positive direction is not allowed." },
        //        { 17181, @"Braking time in negative direction not allowed The value for the braking time in negative direction is not allowed." },
        //        { 17182, @"Switching time from high to low speed not allowed The value for the switching time from high to low speed is not allowed." },
        //        { 17183, @"Creep distance for stop not allowed The value for the creep distance for an explicit stop is not allowed." },
        //        { 17184, @"Motion monitoring time not allowed The value for the motion monitoring time is not allowed." },
        //        { 17185, @"Position window monitoring not allowed The value for the activation of the position window monitoring is not allowed." },
        //        { 17186, @"Target window monitoring not allowed The value for the activation of target window monitoring is not allowed." },
        //        { 17187, @"Loop not allowed The value for the activation of loop movement is not allowed." },
        //        { 17188, @"Motion monitoring time not allowed The value for the motion monitoring time is not allowed." },
        //        { 17189, @"Target window range not allowed The value for the target window is not allowed." },
        //        { 17190, @"Position window range not allowed The value for the position window is not allowed." },
        //        { 17192, @"Loop movement not allowed The value for the loop movement is not allowed." },
        //        { 17193, @"Axis cycle time not allowed The value for the axis cycle time is not allowed." },
        //        { 17194, @"Stepper motor operating mode not allowed The value for the stepper motor operating mode is not allowed." },
        //        { 17195, @"Displacement per stepper motor step not allowed The value for the displacement associated with one step of the stepper motor is not allowed (step scaling)." },
        //        { 17196, @"Minimum speed for stepper motor set value profile not allowed The value for the minimum speed of the stepper motor speed profile is not allowed." },
        //        { 17197, @"Stepper motor steps per speed level not allowed The value for the number of steps per speed level of the setpoint generation is not allowed." },
        //        { 17198, @"DWORD for the interpretation of the axis units not allowed The value containing the flags for the interpretation of position and velocity units is not allowed." },
        //        { 17199, @"Maximum velocity not allowed The value for the maximum permitted velocity is not allowed." },
        //        { 17200, @"Motion monitoring window not allowed The value for the motion monitoring window is not allowed." },
        //        { 17201, @"PEH time monitoring not allowed The value for the activation of the PEH time monitoring is not allowed (PEH: positioning end and halt)." },
        //        { 17202, @"PEH monitoring time not allowed The value for the PEH monitoring time (timeout) is not allowed (PEH: positioning end and halt). Default value: 5s" },
        //        { 17203, @"Parameter Brake Release Delay is invalid
        //        The parameter for the brake release delay of a high/low speed axis is invalid." },
        //        { 17204, @"Parameter NC Data Persistence is invalid
        //        The boolean parameter NC Data Persistence of an axis is invalid." },
        //        { 17205, @"Parameter for Error Reaction Mode is invalid
        //        The parameter for the error reaction mode of the axis is invalid (instantaneous, delayed)." },
        //        { 17206, @"Parameter for the Error Reaction Delay is invalid
        //        The parameter for the error reaction delay of the axis is invalid." },
        //        { 17207, @"Parameter Use actual values in deactivated state is invalid
        //        The parameter Use actual values in deactivated state is invalid." },
        //        { 17208, @"Parameter Allow Motion Commands for Slave Axes is invalid
        //        The boolean parameter Allow Motion Commands for Slave Axes is invalid. This parameter determines whether a motion command may be sent to a slave axis or whether this is rejected with a NC error 0x4266 or 0x4267." },
        //        { 17209, @"Parameter Allow Motion Commands for axis in external setpoint generation is invalid
        //        The boolean parameter Allow Motion Commands for axis in external setpoint generation is invalid. This parameter determines whether a motion command may be sent to an axis in the external setpoint generation state or whether this is rejected with an error 0x4257." },
        //        { 17210, @"Parameter Fading Acceleration is invalid
        //        The Fading Acceleration parameter for the fading profile from SET to ACTUAL values is invalid. This parameter defines how to crossfade from a setpoint based axis coupling to an actual value based coupling (indirectly results in a time for the crossfade).
        //        Note: The value 0.0 causes the minimum of the default acceleration and default deceleration to be used internally in the NC as the fading acceleration." },
        //        { 17211, @"'Fast Axis Stop' signal type not allowed The value for the signal type of the 'Fast Axis Stop' is not allowed [0...5]." },
        //        { 17216, @"Axis initialization Axis has not been initialized. Although the axis has been created, the rest of the initialization has not been performed (1. Initialization of axis I/O, 2. Initialization of axis, 3. Reset axis)." },
        //        { 17217, @"Group address Axis does not have a group, or the group address has not been initialized (group contains the setpoint generation)." },
        //        { 17218, @"Encoder address The axis does not have an encoder, or the encoder address has not been initialized." },
        //        { 17219, @"Controller address An axis does not have a controller, or the controller address has not been initialized." },
        //        { 17220, @"Drive address The axis does not have a drive, or the drive address has not been initialized." },
        //        { 17221, @"Axis interface PLC to NC address Axis does not have an axis interface from the PLC to the NC, or the axis interface address has not been initialized." },
        //        { 17222, @"Axis interface NC to PLC address Axis does not have an axis interface from the NC to the PLC, or the axis interface address has not been initialized." },
        //        { 17223, @"Size of axis interface NC to PLC not allowed (INTERNAL ERROR) The size of the axis interface from the NC to the PLC (NC to PLC) is not allowed." },
        //        { 17224, @" Size of axis interface PLC to NC not allowed(INTERNAL ERROR) The size of the axis interface from the PLC to the NC is not allowed." },
        //        { 17238, @"Controller enable Controller enable for the axis is not present (see axis interface SPS®NC). This enable is required, for instance, for an axis positioning task." },
        //        { 17239, @"Feed enable negative: There is no feed enable for negative motion direction (see axis interface SPS->NC). This enable is required, for instance, for an axis positioning task in the negative direction." },
        //        { 17240, @"Feed enable plus Feed enable for movement in the positive direction is not present (see axis interface SPS®NC). This enable is required, for instance, for an axis positioning task in the positive direction." },
        //        { 17241, @"Set velocity not allowed The set velocity requested for a positioning task is not allowed. This can happen if the velocity is less than or equal to zero, larger than the maximum permitted axis velocity, or, in the case of servo-drives, is larger than the reference velocity of the axis (see axis and drive parameters)." },
        //        { 17242, @"Movement smaller than one encoder increment (INTERNAL ERROR) The movement required of an axis is, in relation to a positioning task, smaller than one encoder increment (see scaling factor). This information is, however, handled internally in such a way that the positioning is considered to have been completed without an error message being returned." },
        //        { 17243, @"Set acceleration monitoring (INTERNAL ERROR) The set acceleration has exceeded the maximum permitted acceleration or deceleration parameters of the axis" },
        //        { 17244, @"PEH time monitoring The PEH time monitoring has detected that, after the PEH monitoring time that follows a positioning has elapsed, the target position window has not been reached. The following points must be checked: Is the PEH monitoring time, in the sense of timeout monitoring, set to a sufficiently large value (e.g. 1-5 s)? The PEH monitoring time must be chosen to be significantly larger than the target position monitoring time. Have the criteria for the target position monitoring (range window and time) been set too strictly? Note: The PEH time monitoring only functions when target position monitoring is active!" },
        //        { 17245, @"Motion Monitoring
        //        The actual position of the axis has not changed or has changed only slightly during the motion monitoring time. To avoid an error, the axis must change by more than the parameterized motion monitoring window in at least one NC cycle during the monitoring time.
        //        => Check, whether axis is mechanically blocked, or the encoder system failed." },
        //        { 17246, @"Looping distance less than breaking distance The absolute value of the looping distance is less or equal than the positive or negative breaking distance. This is not allowed." },
        //        { 17247, @"Starting velocity not allowed
        //        The required starting velocity for a positioning task is not allowed (normally the starting velocity is zero). This can happen if the velocity is less than or equal to zero, larger than the maximum permitted axis velocity, or, in the case of servo drives, is larger than the reference velocity of the axis (see axis and drive parameters)." },
        //        { 17248, @"Final velocity not allowed
        //        The required final velocity for a positioning task is not allowed (normally the final velocity is zero). This can happen if the velocity is less than or equal to zero, larger than the maximum permitted axis velocity, or, in the case of servo drives, is larger than the reference velocity of the axis (see axis and drive parameters)." },
        //        { 17249, @"Time range exceeded (future) The calculated position lies too far in the future (e.g. when converting a position value in a DC time stamp)." },
        //        { 17250, @"Time range exceeded (past) The calculated position lies too far in the past (e.g. when converting a position value in a DC time stamp)." },
        //        { 17251, @"Position cannot be determined The requested position cannot be determined. Case 1: It was not passed through in the past. Case 2: It cannot be reached in future. A reason can be a zero velocity value or an acceleration that causes a motion reversal." },
        //        { 17252, @"Position indeterminable (conflicting direction of travel) The direction of travel expected by the caller of the function deviates from the actual direction of travel (conflict between PLC and NC view, for example when converting a position to a DC time)." },
        //        { 17264, @"No slave coupling possible (velocity violation)
        //        A slave coupling to a master axis (e.g. by a universal flying saw) is rejected because otherwise the maximum velocity of the slave axis would be exceeded (a velocity monitoring has been selected)." },
        //        { 17265, @"No slave coupling possible (acceleration violation)
        //        A slave coupling to a master axis (e.g. by a universal flying saw) is rejected, because otherwise the maximum acceleration of the slave axis will be exceeded (an acceleration monitoring is selected)." },
        //        { 17266, @"See TF5055 NC Flying Saw - Error Codes" },
        //        { 17312, @"Axis consequential error Consequential error resulting from another causative error related to another axis. Axis consequential errors can occur in relation to master/slave-couplings or with multiple axis interpolating DXD groups" },
        //    };

        //}



    }
    
}


