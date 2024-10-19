import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstScriptmanagementexceptionviewComponent } from './sbc-mst-scriptmanagementexceptionview.component';

describe('SbcMstScriptmanagementexceptionviewComponent', () => {
  let component: SbcMstScriptmanagementexceptionviewComponent;
  let fixture: ComponentFixture<SbcMstScriptmanagementexceptionviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstScriptmanagementexceptionviewComponent]
    });
    fixture = TestBed.createComponent(SbcMstScriptmanagementexceptionviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
