import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstScriptmanagementComponent } from './sbc-mst-scriptmanagement.component';

describe('SbcMstScriptmanagementComponent', () => {
  let component: SbcMstScriptmanagementComponent;
  let fixture: ComponentFixture<SbcMstScriptmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstScriptmanagementComponent]
    });
    fixture = TestBed.createComponent(SbcMstScriptmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
