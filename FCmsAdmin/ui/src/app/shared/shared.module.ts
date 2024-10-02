import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SlidePanelComponent } from './slideout/slideout-panel.component';
import { StructureComponent } from './structure/structure.component';
import { ContentdefinitionComponent } from './structure/content-definition.component';

@NgModule({ declarations: [
        SlidePanelComponent,
        StructureComponent,
        ContentdefinitionComponent
    ],
    exports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        SlidePanelComponent,
        StructureComponent
    ], imports: [CommonModule,
        FormsModule,
        BrowserAnimationsModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class SharedModule { }
