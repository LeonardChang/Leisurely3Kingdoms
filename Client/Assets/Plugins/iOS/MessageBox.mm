//
//  MessageBox.mm
//  IOSMessageBox
//
//  Created by Coder on 12-6-27.
//  Copyright (c) 2012年 __MyCompanyName__. All rights reserved.
//

@implementation Test

extern "C" void MessageBox(const char* _title, const char* _message, const char* _button)
{
    NSString *title = [[NSString alloc] initWithCString:_title];
    NSString *message = [[NSString alloc] initWithCString:_message];
    NSString *button = [[NSString alloc] initWithCString:_button];
    
    UIAlertView *alert = [[UIAlertView alloc] init];
    [alert setTitle:title];
    [alert setMessage:message];
    [alert addButtonWithTitle:button];
    [alert show];
    [alert release];
     
    [title release];
    [message release];
    [button release];
}

@end
