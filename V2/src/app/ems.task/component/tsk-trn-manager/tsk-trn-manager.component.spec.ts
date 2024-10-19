import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TskTrnManagerComponent } from './tsk-trn-manager.component';

describe('TskTrnManagerComponent', () => {
  let component: TskTrnManagerComponent;
  let fixture: ComponentFixture<TskTrnManagerComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TskTrnManagerComponent]
    });
    fixture = TestBed.createComponent(TskTrnManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
