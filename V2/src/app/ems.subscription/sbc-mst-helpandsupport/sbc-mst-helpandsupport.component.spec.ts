import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SbcMstHelpandsupportComponent } from './sbc-mst-helpandsupport.component';

describe('SbcMstHelpandsupportComponent', () => {
  let component: SbcMstHelpandsupportComponent;
  let fixture: ComponentFixture<SbcMstHelpandsupportComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SbcMstHelpandsupportComponent]
    });
    fixture = TestBed.createComponent(SbcMstHelpandsupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
