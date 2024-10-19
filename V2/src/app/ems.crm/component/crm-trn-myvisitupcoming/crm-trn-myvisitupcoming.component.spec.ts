import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnMyvisitupcomingComponent } from './crm-trn-myvisitupcoming.component';

describe('CrmTrnMyvisitupcomingComponent', () => {
  let component: CrmTrnMyvisitupcomingComponent;
  let fixture: ComponentFixture<CrmTrnMyvisitupcomingComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnMyvisitupcomingComponent]
    });
    fixture = TestBed.createComponent(CrmTrnMyvisitupcomingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
