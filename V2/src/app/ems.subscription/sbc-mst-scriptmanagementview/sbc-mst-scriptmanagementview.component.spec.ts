import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstScriptmanagementviewComponent } from './sbc-mst-scriptmanagementview.component';

describe('SbcMstScriptmanagementviewComponent', () => {
  let component: SbcMstScriptmanagementviewComponent;
  let fixture: ComponentFixture<SbcMstScriptmanagementviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstScriptmanagementviewComponent]
    });
    fixture = TestBed.createComponent(SbcMstScriptmanagementviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
